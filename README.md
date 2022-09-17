## BinaryToText
Dump binary file using formatter file
書式ファイルを使用してバイナリーファイルをダンプする

-----
Usage)
  BinaryToText -i [Binary data path] -o [Text(dump) path] (-p [Dump format path])

Ex)
  BinaryToText -i Dump.dat -o Output.ttxt -p Format.json

- Binary data path
Binary file path to dump
ダンプするバイナリーファイルパス
- Text dump path
File path for dump output
バイナリーファイルをテキスト形式にして出力するファイルパス
- Dump format path(option)
Dump output format(json file)
バイナリーファイルをダンプする時に使用する書式
This is unnecessary if the dump format file named binary file name.json exists in the same folder as the binary file
Ex) InBinary.dat(Binary) <-> InBinary.json(Format)
バイナリファイル名.jsonというダンプフォーマットファイルがバイナリファイルと同じフォルダーに存在すればこれは不要
例) InBinary.dat(Binary) <-> InBinary.json(Format)
------
Format.json
  1. encoding is utf-8
  1. Format(詳細はPrograms.csを参照)
```
  {
    "title_separater" : " : ",
    "items" : [
      {"title" : "", "type" : "byte", "size" : 1, "format" : "D"}
    ]
  }
```



