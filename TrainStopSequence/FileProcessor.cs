namespace TrainStopSequence
{
    /*
     * FileProcessor processes the file contents and stores the result
     * in the Station object to create the stop sequence from the provided file
     */
    public class FileProcessor
    {
        private readonly StopSequenceAnalyzer stoppingSequence;

        public FileProcessor(StopSequenceAnalyzer stoppingSequence)
        {
            this.stoppingSequence = stoppingSequence;
        }
        public void ProcessFile(string filePath)
        {
            try
            {
                // Read and store the file contents
                string fileContents = File.ReadAllText(filePath);

                // Store the stations of a train stop sequence by parsing the file contents
                List<Station> stopSequence = ParseFileContents(fileContents);

                string description = stoppingSequence.StopSequenceDescriptionBuilder(stopSequence);

                // Print the train stop sequence description
                Console.WriteLine("\n" + description);
                Console.WriteLine("\n**********************************************");
                Console.WriteLine("Thank you for travelling with Queensland Rail!");
                Console.WriteLine("**********************************************");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing stop sequence: {ex.Message}");
            }
        }
        
        private List<Station> ParseFileContents(string fileContents)
        {
            // Stores the stations of a train stop sequence
            List<Station> stopSequence = new List<Station>();

            // Split the file contents into individual station entries
            string[] stationEntries = fileContents.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (string stationEntry in stationEntries)
            {
                string[] parts = stationEntry.Trim().Split(',');

                // Check if the length of the station entry two parts
                if (parts.Length == 2)
                {
                    string stationName = parts[0].Trim();
                    bool isStopping = bool.TryParse(parts[1], out bool result) ? result : false;

                    // Create a Station object and add it to the stop sequence
                    stopSequence.Add(new Station(stationName, isStopping));
                }
                else
                {
                    Console.WriteLine($"Invalid station entry: {stationEntry}");
                }
            }
            
            List<Station> stoppingStations = stopSequence.Where(s => s.IsStopping).ToList();
            Station lastStoppingStation = stoppingStations.Last();

            if (stopSequence.Last() != lastStoppingStation)
            {
                stopSequence.RemoveAll(x => stopSequence.IndexOf(x) > stopSequence.IndexOf(lastStoppingStation));
            }

            return stopSequence;
        }
    }
}
