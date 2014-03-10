using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BerpGherkinParser
{
    public class FastSourceReader : ISourceReader
    {
        private const int BUFFER_SIZE = 1024 * 2;
        private const double INITIAL_CONCAT_BUFFER_RATIO = 1.4;
        private const double EXTEND_CONCAT_BUFFER_RATIO = 1.2;

        private int lineNo = 0;
        private readonly TextReader innerReader;

        private readonly char[] buffer;
        private int bufferPosition = 1;
        private int bufferContentSize = 1;

        private char[] concatBuffer;

        public static int ContactBufferInitCount = 0;
        public static int ContactBufferUsageCount = 0;
        public static int IncreaseCount = 0;
        public static int ReaderCount = 0;

        public FastSourceReader(TextReader innerReader)
        {
            ReaderCount++;

            this.innerReader = innerReader;
            buffer = new char[BUFFER_SIZE];
        }

        private void EnsureContactBuffer(int requiredSize, int usedLength)
        {
            if (concatBuffer == null)
            {
                ContactBufferInitCount++;
                concatBuffer = new char[(int)(requiredSize * INITIAL_CONCAT_BUFFER_RATIO)];
            }
            else if (concatBuffer.Length < requiredSize)
            {
                IncreaseCount++;
                var newContactBuffer = new char[(int)(requiredSize * EXTEND_CONCAT_BUFFER_RATIO)];
                if (usedLength > 0)
                    Array.Copy(concatBuffer, 0, newContactBuffer, 0, usedLength);
                concatBuffer = newContactBuffer;
            }
        }

        public ISourceLine ReadLine()
        {
            if (bufferContentSize == 0)
                return null;

            bool useConcatBuffer = false;
            int lineLength = 0;
            int startPos;

            do
            {
                if (bufferPosition == bufferContentSize) // everything processed from buffer
                {
                    bufferContentSize = innerReader.ReadBlock(buffer, 0, BUFFER_SIZE);
                    bufferPosition = 0;
                }

                startPos = bufferPosition;
                bool lineEndFound = false;
                int appendLength;

                while (bufferPosition < bufferContentSize)
                {
                    char c = buffer[bufferPosition++];

                    if (c == '\n')
                    {
                        appendLength = bufferPosition - startPos - 1;
                        if (appendLength > 0 && buffer[startPos + appendLength - 1] == '\r')
                            appendLength--;

                        if (useConcatBuffer && appendLength > 0)
                        {
                            // append line part to concat buffer
                            EnsureContactBuffer(lineLength + appendLength, lineLength); 
                            Array.Copy(buffer, startPos, concatBuffer, lineLength, appendLength);
                        }
                        lineLength += appendLength;

                        lineEndFound = true;
                        break;
                    }
                }
                if (lineEndFound)
                    break;

                // no \n found in the buffer, we copy it to the concatBuffer

                appendLength = bufferContentSize - startPos;
                if (appendLength > 0 && buffer[bufferContentSize - 1] == '\r') // if the buffer was split between \r and \n, we ignore the \r at the buffer end
                    appendLength--;

                // checking for appendLength > 0 causes strange memory consuption, maybe because of the usual empty lines at EOF
                EnsureContactBuffer(lineLength + appendLength, lineLength); 
                Array.Copy(buffer, startPos, concatBuffer, lineLength, appendLength);
                lineLength += appendLength;
                useConcatBuffer = true;
                ContactBufferUsageCount++;

            } while (bufferPosition == bufferContentSize && bufferContentSize > 0);

            if (useConcatBuffer)
                return new SourceLine(concatBuffer, lineNo++, 0, lineLength);
            
            return new SourceLine(buffer, lineNo++, startPos, lineLength);
        }

        private class SourceLine : ISourceLine
        {
            private char[] buffer;
            private readonly int lineNo;
            private readonly int startIndex;
            private readonly int lineLength;
            private readonly int indent = 0;

            public SourceLine(char[] buffer, int lineNo, int startIndex, int lineLength)
            {
                this.buffer = buffer;
                this.lineNo = lineNo;
                this.startIndex = startIndex;
                this.lineLength = lineLength;
                while (indent < lineLength && IsWhiteSpace(buffer[startIndex + indent]))
                {
                    indent++;
                }
            }

            private bool IsWhiteSpace(char c)
            {
                return char.IsWhiteSpace(c) || (c == 65279);
            }

            public int Indent
            {
                get { return indent; }
            }

            public bool IsEmpty()
            {
                return indent == lineLength;
            }

            public bool StartsWith(string text)
            {
                int pos = 0;
                int textLenght = text.Length;
                if (indent + textLenght > lineLength)
                    return false;

                while (pos < textLenght && text[pos] == buffer[startIndex + indent + pos])
                {
                    pos++;
                }

                return pos == textLenght;
            }

            public bool StartsWith(string text, char separator)
            {
                int textLenght = text.Length;
                if (indent + textLenght + 1 > lineLength)
                    return false;

                return StartsWith(text) && buffer[startIndex + indent + textLenght] == separator;
            }

            public string GetRestTrimmed(int length)
            {
                int restLength = lineLength - length - indent;
                while (restLength >0 && IsWhiteSpace(buffer[startIndex + indent + restLength - 1]))
                {
                    restLength--;
                }
                return new string(buffer, startIndex + indent + length, restLength);
            }

            public IEnumerable<string> Split(int prefixToRemove)
            {
                int pos = indent;
                while (pos < lineLength)
                {
                    while (pos < lineLength && IsWhiteSpace(buffer[startIndex + pos]))
                    {
                        pos++;
                    }
                    pos += prefixToRemove;
                    if (pos > lineLength)
                        pos = lineLength;
                    int partIndex = pos;
                    while (pos < lineLength && !IsWhiteSpace(buffer[startIndex + pos]))
                    {
                        pos++;
                    }
                    yield return new string(buffer, startIndex + partIndex, pos - partIndex);
                }
            }

            public string GetLineText()
            {
                return new string(buffer, startIndex, lineLength);
            }

            public IEnumerable<string> GetTableCells()
            {
                int pipePos = IndexOf('|', startIndex + indent);
                if (pipePos < 0)
                    yield break;//error  if there was a non-ws

                pipePos -= startIndex;

                while (pipePos < lineLength)
                {
                    int pos = pipePos + 1;
                    while (pos < lineLength && IsWhiteSpace(buffer[startIndex + pos]))
                    {
                        pos++;
                    }
                    int partIndex = pos;
                    while (pos < lineLength && buffer[startIndex + pos] != '|')
                    {
                        pos++;
                    }
                    pipePos = pos == lineLength ? -1 : pos;
                    if (pipePos < 0)
                        yield break; //error  if there was a non-ws
                    while (pos > partIndex && IsWhiteSpace(buffer[startIndex + pos]))
                    {
                        pos--;
                    }
                    yield return new string(buffer, startIndex + partIndex, pos - partIndex);
                }
            }

            private int IndexOf(char c, int startPos)
            {
                while (startPos < startIndex + lineLength && buffer[startPos] != c)
                {
                    startPos++;
                }
                return startPos == startIndex + lineLength ? -1 : startPos;
            }

            public string GetLineText(int multiLineArgumentIndent)
            {
                int startPos = 0;

                while (startPos < multiLineArgumentIndent && startPos < lineLength &&
                    IsWhiteSpace(buffer[startIndex + startPos]))
                {
                    startPos++;
                }
                return new string(buffer, startIndex + startPos, lineLength - startPos);
            }

            public void Detach()
            {
                buffer = (char[])buffer.Clone();
            }
        }
    }
}
