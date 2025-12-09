#:property ImplicitUsings=enable
#:property Nullable=enable
#:package Humanizer@3.0.1

using System.IO.Compression;
using System.Text.RegularExpressions;
using Humanizer;

var zipFileName = $"{Path.GetFileName(Directory.GetCurrentDirectory())}.zip";

Console.WriteLine($"Building pack: {zipFileName} ...");

if (File.Exists(zipFileName)) File.Delete(zipFileName);

await using var fileWriter = File.Create(zipFileName);
await using ZipArchive zipArchive = new(fileWriter, ZipArchiveMode.Create);

foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories)
	.Where(f => Regex.IsMatch(Path.GetFileName(f), @"^([\w\.\- ]+\.(?:png|mcmeta)|license\.txt)$", RegexOptions.IgnoreCase)))
	await zipArchive.CreateEntryFromFileAsync(file,
		Path.GetRelativePath(Environment.CurrentDirectory, file).Replace('\\', '/'), CompressionLevel.SmallestSize);

Console.WriteLine($"Build complete ({ByteSize.FromBytes(new FileInfo(zipFileName).Length).ToString("0.##")})");