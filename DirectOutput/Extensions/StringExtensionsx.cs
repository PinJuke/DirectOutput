﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.IO;
//using System.Web;


/// <summary>
/// Extends the String object with additional functionality.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a Hexnumber to int
    /// </summary>
    /// <returns>Int of given Hexnumber</returns>
    public static int HexToInt(this string s)
    {
        return int.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
    /// <summary>
    /// Converts a Hexnumber to byte
    /// </summary>
    /// <returns>Byte of given Hexnumber</returns>
    public static int HexToByte(this string s)
    {
        return byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
    /// <summary>
    /// Determines whether the string is a hex number.
    /// </summary>
    /// <returns>true if the string contains only hexchars, otherwise true</returns>
    public static bool IsHexString(this string s)
    {
        return s.IsHexString(0, s.Length);
    }

    /// <summary>
    /// Determines whether the specified part of the string is a hex number.
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="startindex">The startindex of the substring to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified part of the string is a hex number, otherwise <c>false</c>.
    /// </returns>
    public static bool IsHexString(this string s, int startindex)
    {
        return s.IsHexString(startindex, s.Length - startindex);
    }

    /// <summary>
    /// Determines whether the specified part of the string is a hex number.
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="startindex">The startindex of the substring to check.</param>
    /// <param name="length">The length of the substrng to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified part of the string is a hex number, otherwise <c>false</c>.
    /// </returns>
    public static bool IsHexString(this string s, int startindex, int length)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;

        if (startindex + length > s.Length) return false;

        return System.Text.RegularExpressions.Regex.IsMatch(s.Substring(startindex, length), @"\A\b[0-9a-fA-F]+\b\Z");
    }




    /// <summary>
    /// Converts the string to a byte array.
    /// </summary>
    /// <returns>Return a UTF8 encoded byte array with the contents of the string</returns>
    public static byte[] ToByteArray(this string s)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetBytes(s);
    }

    /// <summary>
    /// Converts the string to a StringBuilder object.
    /// </summary>
    /// <returns>StringBuilder object containing the contents of the string.</returns>
    public static StringBuilder ToStringBuilder(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return new StringBuilder("");
        }
        return new StringBuilder(s);
    }

    /// <summary>
    /// Returns a number of characters from the left-hand side of this instance. 
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="length">The number of characters to return.</param>
    /// <returns>Returns a string containing the leftmost <paramref name="length"/> characters of the string.</returns>
    public static string Left(this string s, int length)
    {
        return s.Substring(0, length);
    }

    /// <summary>
    /// Returns a number of characters from the right-hand side of this instance. 
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="length">The number of characters to return.</param>
    /// <returns>Returns a string containing the rightmost <paramref name="length"/> characters of the string.</returns>
    public static string Right(this string s, int length)
    {
        return s.Substring(s.Length - length, length);
    }

    /// <summary>
    /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length. 
    /// <para>The behaviour of this method is the same as substring.</para>
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
    /// <param name="length">The number of characters in the substring. </param>
    /// <returns>A string that is equivalent to the substring of <paramref name="length"/> that begins at <paramref name="startIndex"/> in this instance, or Empty if <paramref name="startIndex"/> is equal to the length of this instance </returns>
    public static string Mid(this string s, int startIndex, int length)
    {
        return s.Substring(startIndex, length);
    }

    /// <summary>
    /// Converts the string to int
    /// </summary>
    /// <returns>The int value of the string.If the string can not be converted to int, the return value will be 0.</returns>
    public static int ToInteger(this string s)
    {
        int integerValue = 0;
        int.TryParse(s, out integerValue);
        return integerValue;
    }

    /// <summary>
    /// Converts the string to uint
    /// </summary>
    /// <returns>The uint value of the string. <para>If the string can not be converted to uint, the return value will be 0.</para></returns>
    public static uint ToUInt(this string s)
    {
        uint uintegerValue = 0;
        uint.TryParse(s, out uintegerValue);
        return uintegerValue;
    }

    /// <summary>
    /// Indicates whether the string contains a integer value
    /// </summary>
    /// <returns>true if the string can be converted to int, otherwise false.</returns>
    public static bool IsInteger(this string s)
    {
        int dummy;
        if (s.IsNullOrWhiteSpace()) return false;
        return int.TryParse(s, out dummy);
    }

    /// <summary>
    /// Indicates whether the string contains a unsigned integer value
    /// </summary>
    /// <returns>true if the string can be converted to uint, otherwise false.</returns>
    public static bool IsUInt(this string s)
    {
        uint dummy;
        if (s.IsNullOrWhiteSpace()) return false;
        return uint.TryParse(s, out dummy);
    }


    /// <summary>
    /// Indicates whether the string is Nothing or an Empty string.
    /// </summary>
    /// <returns>true if the string is Nothing or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    /// <summary>
    /// Indicates whether the string is Nothing, empty, or consists only of white-space characters.
    /// </summary>
    /// <returns>true if the string is Nothing or String.Empty, or if value consists exclusively of white-space characters. </returns>
    public static bool IsNullOrWhiteSpace(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }




    /// <summary>
    /// Replaces the format item in the string with the string representation of a corresponding object in a specified array.
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="args">An object array that contains zero or more objects to format. </param>
    /// <returns>A copy of the string in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
    public static string Build(this string s, params object[] args)
    {
        if (s == null) { return ""; }
        return string.Format(s, args);
    }




    /// <summary>
    /// Indicates if the string is a valid email address.
    /// <para>Only the structure of the string is checked. No checks on existance of the domain or email address are performed.</para>
    /// </summary>
    /// <param name="s">The string</param>
    /// <returns>true if the string is a valid email address, otherwise false.</returns>
    public static bool IsEmail(this string s)
    {
        Regex MailCheck = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        return MailCheck.IsMatch(s);
    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="FileName">The complete file path to write to.<para>
    /// If the file does not exist a new file will be created. If the file exists, the file will be overwritten.</para> </param>
    public static void WriteToFile(this string s, string FileName)
    {
        s.WriteToFile(FileName, false);
    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="FileName">The complete file path to write to. </param>
    /// <param name="Append">Determines whether data is to be appended to the file. If the file exists and append is false, the file is overwritten. If the file exists and append is true, the data is appended to the file. Otherwise, a new file is created. </param>
    public static void WriteToFile(this string s, string FileName, bool Append)
    {
        TextWriter tw = null;
        try
        {
            tw = new StreamWriter(FileName, Append);
            tw.Write(s);
        }
        catch (Exception)
        {

            if (tw != null)
            {
                tw.Close();
            }
            throw;
        }

        tw.Close();


    }



    /// <summary>
    /// Replaces all occurences of a string with another string.
    /// </summary>
    /// <param name="originalString">The original string.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns>String with replaced values.</returns>
    static public string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
    {
        StringBuilder sb = new StringBuilder();

        int previousIndex = 0;
        int index = originalString.IndexOf(oldValue, comparisonType);
        while (index != -1)
        {
            sb.Append(originalString.Substring(previousIndex, index - previousIndex));
            sb.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = originalString.IndexOf(oldValue, index, comparisonType);
        }
        sb.Append(originalString.Substring(previousIndex));

        return sb.ToString();

    }

    /// <summary>
    /// Gets the bytes in a string.
    /// </summary>
    /// <returns>Array of bytes of the string.</returns>
    static public byte[] GetBytes(this string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

}






