﻿namespace System.Xml
{
    using System;

    internal class Ucs4Decoder1234 : Ucs4Decoder
    {
        internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            byteCount += byteIndex;
            int index = byteIndex;
            int num3 = charIndex;
            while ((index + 3) < byteCount)
            {
                uint code = (uint) ((((bytes[index] << 0x18) | (bytes[index + 1] << 0x10)) | (bytes[index + 2] << 8)) | bytes[index + 3]);
                if (code > 0x10ffff)
                {
                    throw new ArgumentException(Res.GetString("Enc_InvalidByteInEncoding", new object[] { index }), null);
                }
                if (code > 0xffff)
                {
                    base.Ucs4ToUTF16(code, chars, num3);
                    num3++;
                }
                else
                {
                    if (XmlCharType.IsSurrogate((int) code))
                    {
                        throw new XmlException("Xml_InvalidCharInThisEncoding", string.Empty);
                    }
                    chars[num3] = (char) code;
                }
                num3++;
                index += 4;
            }
            return (num3 - charIndex);
        }
    }
}

