﻿/**
 * This utility namespace is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility;

public static class MainUtilities
{


  /// <summary>
  ///   Turns a string into a list of ints.
  /// </summary>
  /// <param name="str">The string to split up</param>
  /// <param name="delimiter">How to split. Default is each character becomes an int.</param>
  /// <returns>A list of integers</returns>
  public static List<int> ToIntList(this string str, string delimiter = "")
  {
    if (delimiter == "")
    {
      List<int> result = new();
      foreach (char c in str)
        if (int.TryParse(c.ToString(), out int n))
          result.Add(n);
      return result;
    }

    return str.Split(delimiter).Where(n => int.TryParse(n, out int v)).Select(n => Convert.ToInt32(n)).ToList();
  }

  /// <summary>
  ///   Turns a string array into an array of int.
  ///   Utilizes ToIntList
  /// </summary>
  /// <param name="array">array of strings to parse</param>
  /// <returns>An array of integers</returns>
  public static int[] ToIntArray(this string[] array)
  {
    return string.Join(",", array).ToIntList(",").ToArray();
  }

  /// <summary>
  ///   Extract all the positive integers from a string, automatically deliminates on all non numeric chars
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<int> ExtractPosInts(this string str)
  {
    return Regex.Matches(str, "\\d+").Select(m => int.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all ints from a string, treats `-` as a negative sign.
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<int> ExtractInts(this string str)
  {
    return Regex.Matches(str, "-?\\d+").Select(m => int.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all ints from a string, treats `-` as a negative sign.
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<long> ExtractLongs(this string str)
  {
    return Regex.Matches(str, "-?\\d+").Select(m => long.Parse(m.Value));
  }

  /// <summary>
  ///   Extract all the positive integers from a string, automatically deliminates on all non numeric chars
  /// </summary>
  /// <param name="str">String to search</param>
  /// <returns>An ordered enumerable of the integers found in the string.</returns>
  public static IEnumerable<long> ExtractPosLongs(this string str)
  {
    return Regex.Matches(str, "\\d+").Select(m => long.Parse(m.Value));
  }

  /// <summary>
  ///   Extracts all "Words" (including xnoppyt) from a string
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static IEnumerable<string> ExtractWords(this string str)
  {
    return Regex.Matches(str, "[a-zA-z]+").Select(a => a.Value);
  }

  /// <summary>
  ///   Turns a string into a list of longs.
  /// </summary>
  /// <param name="str">The string to split up</param>
  /// <param name="delimiter">How to split. Default is each character becomes a long.</param>
  /// <returns>A list of long</returns>
  public static List<long> ToLongList(this string str, string delimiter = "")
  {
    if (delimiter == "")
    {
      List<long> result = new();
      foreach (char c in str)
        if (long.TryParse(c.ToString(), out long n))
          result.Add(n);
      return result.ToList();
    }

    return str.Split(delimiter).Where(n => long.TryParse(n, out long v)).Select(n => Convert.ToInt64(n)).ToList();
  }

  /// <summary>
  ///   Repeat text n times, returning a string with the repeated text.
  /// </summary>
  /// <param name="text">Text to Repeat.</param>
  /// <param name="n">Number of times to repeat.</param>
  /// <param name="seperator"></param>
  /// <returns>Repeated String.</returns>
  public static string Repeat(this string text, int n, string seperator = "")
  {
    return new StringBuilder((text.Length + seperator.Length) * n).Insert(0, $"{text}{seperator}", n).ToString();
  }

  public static IEnumerable<T> Flatten<T>(this T[,] map)
  {
    for (int row = 0; row < map.GetLength(0); row++)
    {
      for (int col = 0; col < map.GetLength(1); col++)
      {
        yield return map[row, col];
      }
    }
  }

  public static string JoinAsStrings<T>(this IEnumerable<T> items, char seperator = '\u0000')
  {
    return string.Join(seperator, items);
  }

  public static string JoinAsStrings<T>(this IEnumerable<T> items, string seperator)
  {
    return string.Join(seperator, items);
  }

  public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
  {
    return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
      .Where(s => blankLines || !string.IsNullOrWhiteSpace(s)).Select(s => shouldTrim ?
        s.Trim() :
        s).ToList();
  }
  public static List<string> SplitByDoubleNewline(this string input, bool blankLines = false, bool shouldTrim = true)
  {
    return input.Split(new[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None)
      .Where(s => blankLines || !string.IsNullOrWhiteSpace(s)).Select(s => shouldTrim ?
        s.Trim() :
        s).ToList();
  }

  /// <summary>
  ///   Splits the input into columns, this is sometimes nice for maps drawing.
  ///   Automatically expands to a full rectangle if needed based on max length and number of rows.
  ///   Empty cells are denoted as ' ' (Space character)
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public static string[] SplitIntoColumns(this string input)
  {
    var rows = input.SplitByNewline(false, false);
    int numColumns = rows.Max(x => x.Length);

    string[] res = new string[numColumns];
    for (int i = 0; i < numColumns; i++)
    {
      StringBuilder sb = new();
      foreach (string row in rows)
      {
        try
        {
          sb.Append(row[i]);
        }
        catch (IndexOutOfRangeException)
        {
          sb.Append(' ');
        }
      }

      res[i] = sb.ToString();
    }

    return res;
  }

  //Removes a specified row AND column from a multidimensional array.
  public static int[,] TrimArray(this int[,] originalArray, int rowToRemove, int columnToRemove)
  {
    int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

    for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
    {
      if (i == rowToRemove)
        continue;

      for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
      {
        if (k == columnToRemove)
          continue;

        result[j, u] = originalArray[i, k];
        u++;
      }

      j++;
    }

    return result;
  }

  public static int ManhattanDistance(this (int x, int y) a, (int x, int y) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
  }

  public static int ManhattanDistance(this (int x, int y, int z) a, (int x, int y, int z) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
  }

  public static long ManhattanDistance(this (long x, long y) a, (long x, long y) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
  }

  public static long ManhattanDistance(this (long x, long y, long z) a, (long x, long y, long z) b)
  {
    return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
  }

  public static long ManhattanMagnitude(this (long x, long y, long z) a)
  {
    return a.ManhattanDistance((0, 0, 0));
  }

  public static double FindGCD(double a, double b)
  {
    if (a == 0 || b == 0) return Math.Max(a, b);

    return a % b == 0 ?
      b :
      FindGCD(b, a % b);
  }

  public static double FindLCM(double a, double b)
  {
    return a * b / FindGCD(a, b);
  }

  public static long FindGCD(long a, long b)
  {
    if (a == 0 || b == 0) return Math.Max(a, b);

    return a % b == 0 ?
      b :
      FindGCD(b, a % b);
  }
  public static long FindLCM(long a, long b)
  {
    return a * b / FindGCD(a, b);
  }

  public static (long gcd, long x, long y) ExtendedGCD(long a, long b)
  {
    if (b == 0) return (a, 1, 0);

    (long gcd0, long x0, long y0) = ExtendedGCD(b, b % a);
    return (gcd0, y0, x0 - a / b * y0);
  }

  public static int Mod(int x, int m)
  {
    int r = x % m;
    return r < 0 ?
      r + m :
      r;
  }

  public static long Mod(long x, long m)
  {
    long r = x % m;
    return r < 0 ?
      r + m :
      r;
  }

  public static long ModInverse(long a, long n)
  {
    return ModPower(a, n - 2, n);
  }

  public static long ModPower(long x, long y, long p)
  {
    return (long)BigInteger.ModPow(x, y, p);
  }

  public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
  {
    return values.Count() == 1 ?
      new[] { values } :
      values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v)).ToList();
  }

  public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values, int subcount)
  {
    var comboList = Combinations(values, subcount).ToList();
    foreach (IEnumerable<T> combination in comboList)
    {
      IEnumerable<IEnumerable<T>> perms = Permutations(combination);
      foreach (int i in Enumerable.Range(0, perms.Count())) yield return perms.ElementAt(i);
    }
  }

  // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
  // in lexicographic order (first [0, 1, 2, ..., m-1]).
  private static IEnumerable<int[]> Combinations(int m, int n)
  {
    int[] result = new int[m];
    Stack<int> stack = new(m);
    stack.Push(0);
    while (stack.Count > 0)
    {
      int index = stack.Count - 1;
      int value = stack.Pop();
      while (value < n)
      {
        result[index++] = value++;
        stack.Push(value);
        if (index != m) continue;

        yield return (int[])result.Clone();

        break;
      }
    }
  }

  public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> array, int m)
  {
    if (array.Count() < m)
      throw new ArgumentException("Array length can't be less than number of selected elements");
    if (m < 1)
      throw new ArgumentException("Number of selected elements can't be less than 1");

    T[] result = new T[m];
    foreach (int[] j in Combinations(m, array.Count()))
    {
      for (int i = 0; i < m; i++)
      {
        result[i] = array.ElementAt(j[i]);
      }

      yield return result;
    }
  }

  public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
  {
    for (int i = 0; i < (float)array.Count() / size; i++)
    {
      yield return array.Skip(i * size).Take(size);
    }
  }

  public static IEnumerable<List<T>> SplitAtIndex<T>(this List<T> array, int index)
  {
    if (index == 0) throw new ArgumentException($"{nameof(index)} must be a non-zero integer");

    if (index > 0)
    {
      index %= array.Count;
      yield return array.Take(index).ToList();
      yield return array.Skip(index).ToList();

    }
    else
    {
      index *= -1;
      index %= array.Count;
      yield return array.SkipLast(index).ToList();
      yield return array.TakeLast(index).ToList();
    }
  }

  public static string[] ToStringArray(this char[][] array)
  {
    string[] tmp = new string[array.GetLength(0)];

    for (int i = 0; i < tmp.Length; i++)
    {
      tmp[i] = array[i].JoinAsStrings();
    }

    return tmp;
  }

  /// <summary>
  ///   Rotates an IEnumerable by the requested amount
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="array"></param>
  /// <param name="rotations">
  ///   Number of steps to take, positive numbers move indices down (first item moves to end of array),
  ///   negative numbers move them up (item at end moves to start)
  /// </param>
  /// <returns></returns>
  public static IEnumerable<T> Rotate<T>(this IEnumerable<T> array, int rotations)
  {
    for (int i = 0; i < array.Count(); i++)
    {
      yield return i + rotations >= 0 ?
        array.ElementAt((i + rotations) % array.Count()) :
        array.ElementAt(i + rotations + array.Count());
    }
  }

  public static (int x, int y) Add(this (int x, int y) a, (int x, int y) b)
  {
    return (a.x + b.x, a.y + b.y);
  }

  public static (int x, int y, int z) Add(this (int x, int y, int z) a, (int x, int y, int z) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z);
  }

  public static (int x, int y, int z, int w) Add(this (int x, int y, int z, int w) a, (int x, int y, int z, int w) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
  }

  public static (long x, long y) Add(this (long x, long y) a, (long x, long y) b)
  {
    return (a.x + b.x, a.y + b.y);
  }

  public static (long x, long y, long z) Add(this (long x, long y, long z) a, (long x, long y, long z) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z);
  }

  public static (long x, long y, long z, long w) Add(this (long x, long y, long z, long w) a, (long x, long y, long z, long w) b)
  {
    return (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
  }

  public static IEnumerable<int> AllIndexesOf(this string str, string value)
  {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException("the string to find may not be empty", nameof(value));

    for (int index = 0;; index += value.Length)
    {
      index = str.IndexOf(value, index);
      if (index == -1)
        break;

      yield return index;
    }
  }

  public static string HexStringToBinary(this string Hexstring)
  {
    return string.Join(string.Empty,
      Hexstring.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
  }


  public static CompassDirection Turn(this CompassDirection value, string turnDir, int degrees = 90)
  {
    return turnDir.ToLower() switch
    {
      "l" or "ccw" => (CompassDirection)(((int)value - degrees + 360) % 360),
      "r" or "cw" => (CompassDirection)(((int)value + degrees) % 360),
      _ => throw new ArgumentException("Value must be L, R, CCW, or CW", nameof(turnDir))
    };
  }


  public static T GetDirection<T>(this Dictionary<(int, int), T> values,
    (int, int) location,
    CompassDirection Direction,
    T defaultVal)
  {
    var n = location.MoveDirection(Direction);
    return values.GetValueOrDefault(n, defaultVal);
  }

  public static T GetDirection<T>(this Dictionary<Coordinate2D, T> values,
    Coordinate2D location,
    CompassDirection Direction,
    T defaultVal)
  {
    var n = location.MoveDirection(Direction);
    return values.GetValueOrDefault(n, defaultVal);
  }


  public static List<K> KeyList<K, V>(this Dictionary<K, V> dictionary, bool sorted = false)
  {
    List<K> keyList = [.. dictionary.Keys];

    if (sorted) keyList.Sort();

    return keyList;
  }

  public static List<Coordinate2D> Neighbors(this Coordinate2D val, bool includeDiagonals = false)
  {
    var tmp = new List<Coordinate2D>
    {
      new(val.x - 1, val.y),
      new(val.x + 1, val.y),
      new(val.x, val.y - 1),
      new(val.x, val.y + 1)
    };
    if (includeDiagonals)
    {
      tmp.AddRange(new List<Coordinate2D>
      {
        new(val.x - 1, val.y - 1),
        new(val.x + 1, val.y - 1),
        new(val.x - 1, val.y + 1),
        new(val.x + 1, val.y + 1)
      });
    }

    return tmp;
  }

  public static IEnumerable<Coordinate3D> GetImmediateNeighbors(this Coordinate3D self)
  {
    yield return (self.x + 1, self.y, self.z);
    yield return (self.x - 1, self.y, self.z);
    yield return (self.x, self.y + 1, self.z);
    yield return (self.x, self.y - 1, self.z);
    yield return (self.x, self.y, self.z + 1);
    yield return (self.x, self.y, self.z - 1);
  }

  public static List<Coordinate2D> AStar(Coordinate2D start,
    Coordinate2D goal,
    Dictionary<Coordinate2D, long> map,
    out long Cost,
    bool IncludeDiagonals = false,
    bool IncludePath = true)
  {
    PriorityQueue<Coordinate2D, long> openSet = new();
    Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();

    Dictionary<Coordinate2D, long> gScore = new()
    {
      [start] = 0
    };

    openSet.Enqueue(start, 0);

    while (openSet.TryDequeue(out var cur, out long _))
    {
      if (cur.Equals(goal))
      {
        Cost = gScore[cur];
        return IncludePath ?
          ReconstructPath(cameFrom, cur) :
          null;
      }

      foreach (var n in cur.Neighbors(IncludeDiagonals).Where(a => map.ContainsKey(a)))
      {
        long tentGScore = gScore[cur] + map[n];
        if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
        {
          cameFrom[n] = cur;
          gScore[n] = tentGScore;
          openSet.Enqueue(n, tentGScore + cur.ManDistance(goal));
        }
      }
    }

    Cost = long.MaxValue;
    return null;
  }

  private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
  {
    List<Coordinate2D> res = new()
    {
      current
    };
    while (cameFrom.ContainsKey(current))
    {
      current = cameFrom[current];
      res.Add(current);
    }

    res.Reverse();
    return res;
  }

  public static (Dictionary<Coordinate2D, char> map, int maxX, int maxY) GenerateMap(this string self, bool discardDot = true)
  {
    var lines = self.SplitByNewline();
    int maxX = 0;
    int maxY = lines.Count - 1;
    Dictionary<Coordinate2D, char> res = new();

    for (int i = 0; i < lines.Count; i++)
    {
      maxX = Math.Max(maxX, lines[i].Length - 1);
      for (int j = 0; j < lines[i].Length; j++)
      {
        if (!discardDot || lines[i][j] != '.')
        {
          res[(j, i)] = lines[i][j];
        }
      }
    }

    return (res, maxX, maxY);
  }

  public static string StringFromMap<TValue>(this Dictionary<Coordinate2D, TValue> self,
    int maxX,
    int maxY,
    bool AssumeEmptyIsDot = true)
  {
    StringBuilder sb = new();
    for (int y = 0; y <= maxY; y++)
    {
      for (int x = 0; x <= maxX; x++)
      {
        if (self.TryGetValue((x, y), out var val))
        {
          sb.Append(val);
        }
        else if (AssumeEmptyIsDot)
        {
          sb.Append(".");
        }
        else
        {
          sb.Append(string.Empty);
        }
      }

      sb.Append("\n");
    }

    return sb.ToString();
  }
  public static IEnumerable<string> Lines(this string str)
  {
    using var sr = new StringReader(str ?? string.Empty);

    while (true)
    {
      string? line = sr.ReadLine();
      if (line == null)
        yield break;

      yield return line;
    }
  }

  public static Dictionary<Point2D<int>, char> Cells(this string str, Func<char, bool> filter = null)
  {
    return str.Cells(c => c, filter);
  }

  public static Dictionary<Point2D<int>, T> Cells<T>(this string str, Func<char, T> selector, Func<char, bool> filter = null)
  {
    return str.Lines().SelectMany((l, y) => l.Select((c, x) => (x, y, c))).Where(n => filter?.Invoke(n.c) ?? true)
      .ToDictionary(n => new Point2D<int>(n.x, n.y), n => selector(n.c));
  }

  public static StringMap<T> AsMap<T>(this string str, Func<char, T> selector)
  {
    return new StringMap<T>(str, selector);
  }

  public static StringMap<char> AsMap(this string str)
  {
    return new StringMap<char>(str, c => c);
  }
}