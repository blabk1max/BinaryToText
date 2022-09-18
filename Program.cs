using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BinaryToText {

  internal class Item {
    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("type")] public string VType { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("format")] public string Format { get; set; }

    [JsonPropertyName("encode")] public string Encode { get; set; }
  }

  internal class Dump {
    [JsonPropertyName("title_separater")] public string TitleSeprater { get; set; }

    [JsonPropertyName("items")] public List<Item> Items { get; set; }
  }

  internal class Program {
    private static void ShowUsage() {
      Console.WriteLine("Usage)");
      Console.WriteLine("  BinaryToText -i [Binary data path] -o [Text(dump) path] (-p [Dump format path])");
      Console.WriteLine("Ex)");
      Console.WriteLine("  BinaryToText -i Dump.dat -o Output.txt -p Format.json");
      Console.WriteLine("  BinaryToText -i Dump.dat -o Output.txt (if exists Dump.json)");
    }

    private static string GetParam(string preFix, string[] args) {

      for (int n = 0; n < args.Length; n++) {
        if (preFix.ToLower() == args[n].ToLower()) {
          if ((n + 1) < args.Length) {
            return (args[n + 1]).Trim('"', ' ', '\'');
          }
        }
      }
      return "";
    }

    static void Main(string[] args) {

      var inPath = GetParam("-i", args);
      var outPath = GetParam("-o", args);
      var formatPath = GetParam("-p", args);

      if (!File.Exists(inPath)) {
        Console.WriteLine($"Binary data path({inPath}) not found.");
        ShowUsage();
        return;
      }

      if (File.Exists(outPath)) {
        string pathBase = Path.GetDirectoryName(outPath);
        string pathFileName2 = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_{Path.GetFileName(outPath)}";
        string bakPath = Path.Combine(pathBase, pathFileName2);

        File.Copy(outPath, bakPath);
        Console.WriteLine($"Text dump path({outPath}) already exists. made a backup({bakPath})");
        File.Delete(outPath);
      }

      var path = Path.ChangeExtension(inPath, ".json");
      if ((File.Exists(path)) && (formatPath == "")) {
        formatPath = path;
      }

      if (!File.Exists(formatPath)) {
        Console.WriteLine($"Dump format path({formatPath}) not found.");
        return;
      }

      Console.WriteLine($"Binary data path : {inPath}");
      Console.WriteLine($"Text dump path : {outPath}");
      Console.WriteLine($"Dump format path : {formatPath}");

      var formatJson = File.ReadAllText(formatPath, System.Text.Encoding.UTF8);
      var formatParams = JsonSerializer.Deserialize<Dump>(formatJson);

      if (formatParams == null) {
        Console.WriteLine($"Invalid dump format in ({formatPath}).");
        return;
      }

      var separater = " : ";
      var append = false;

      try {
        using(var reader = new BinaryReader(new FileStream(inPath, FileMode.Open)))
        using(var writer = new System.IO.StreamWriter(outPath, append, System.Text.Encoding.UTF8)) {
          foreach (var item in formatParams.Items) {
            var title = item.Title ?? "";
            var vtype = (item.VType ?? "byte").ToLower();
            var size = item.Size;
            if (size <= 0) {
              size = 1;
            }
            if ((title != "") && (vtype != "skip") && (vtype != "linefeed")) {
              writer.Write(title + separater);
            }
            var lst = new List<string>();
            var format = "";
            byte[] bData;
            var lf = true;
            switch (vtype) {
              case "skip":
                reader.ReadBytes(size);
                lf = false;
                break;
              case "linefeed":
                for (int n = 0; n < size; n++) {
                  writer.Write("\r\n");
                }
                lf = false;
                break;
              case "byte":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadByte();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "sbyte":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadSByte();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "ushort":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadUInt16();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "short":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadInt16();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "ulong":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadUInt32();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "long":
                format = item.Format ?? "D";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadInt32();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "float":
                format = item.Format ?? "F";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadSingle();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "double":
                format = item.Format ?? "F";
                for (int n = 0; n < size; n++) {
                  var b = reader.ReadDouble();
                  lst.Add(b.ToString(format));
                }
                writer.Write(string.Join(",", lst.ToArray()));
                break;
              case "ansistring":
                bData = reader.ReadBytes(size);
                writer.Write(System.Text.Encoding.ASCII.GetString(bData));
                break;
              case "utf8string":
                bData = reader.ReadBytes(size);
                writer.Write(System.Text.Encoding.UTF8.GetString(bData));
                break;
              case "utf16string":
                bData = reader.ReadBytes(size);
                writer.Write(System.Text.Encoding.Unicode.GetString(bData));
                break;
              case "string":
                bData = reader.ReadBytes(size);
                string encode = item.Encode ?? "us-ascii";
                writer.Write(System.Text.Encoding.GetEncoding(encode).GetString(bData));
                break;
            }
            if (lf) {
              writer.Write("\r\n");
            }
          }
        }

      } catch (System.IO.EndOfStreamException ex) {
        Console.WriteLine($"Exception : {ex.Message}");
      } catch (System.Exception ex) {
        Console.WriteLine($"Exception : {ex.Message}");
      }
    }
  }
}