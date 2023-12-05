using TrainStopSequence;

class Program
{
    static void Main()
    {
        string filePath;
        do
        {
            Console.WriteLine("Please enter the absolute path to a text file: ");
            // Read the file path enetered
            filePath = Console.ReadLine();

            // Check if the file path is valid
            if (!IsValidFilePath(filePath))
            {
                Console.WriteLine($"Invalid file path. Please enter a valid path.");
            }
        } while (!IsValidFilePath(filePath));

        StopSequenceAnalyzer stoppingSequence = new StopSequenceAnalyzer();
        FileProcessor fileProcessor = new FileProcessor(stoppingSequence);
        fileProcessor.ProcessFile(filePath);
    }

    static bool IsValidFilePath(string filePath)
    {
        // Check if the file path is not empty and has a valid extension (e.g., ".txt")
        return !string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath) && Path.GetExtension(filePath).Equals(".txt", StringComparison.OrdinalIgnoreCase);
    }
}
