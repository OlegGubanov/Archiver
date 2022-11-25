using Archiver.Algorithms;

ICompressionAlgorithm huffmanAlgorithm = new HuffmanAlgorithm();
//huffmanAlgorithm.CompressFile(@"C:\Users\o-gyb\source\repos\Archiver\Archiver\text.txt", @"C:\Users\o-gyb\source\repos\Archiver\Archiver\huffmanCompressedText.txt");
//huffmanAlgorithm.DecompressFile(@"C:\Users\o-gyb\source\repos\Archiver\Archiver\huffmanCompressedText.txt", @"C:\Users\o-gyb\source\repos\Archiver\Archiver\huffmanDecompressedText.txt");

Console.WriteLine("Доступные команды: compress, decompress");
while (true)
{
    var command = Console.ReadLine();
    switch (command)
    {
        case "compress":
            {
                Console.WriteLine("Введите путь к файлу");
                var path = Console.ReadLine();
                huffmanAlgorithm.CompressFile(path, "compressed" + Path.GetFileName(path));
                Console.WriteLine($"Успешно!");
                break;
            }
        case "decompress":
            {
                Console.WriteLine("Введите путь к файлу");
                var path = Console.ReadLine();
                huffmanAlgorithm.DecompressFile(path, "decompressed" + Path.GetFileName(path));
                Console.WriteLine("Успешно!");
                break;
            }
        default:
            {
                Console.WriteLine("Неизвестная команда. Доступные команды: compress, decompress");
                break;
            }

    }
}