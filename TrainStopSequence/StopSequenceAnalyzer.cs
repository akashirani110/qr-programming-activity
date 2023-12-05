namespace TrainStopSequence
{
    /*
     * StopSequenceAnalyzer parses through the stop sequence and generates the
     * train stop sequence description.
     */
    public class StopSequenceAnalyzer
    {
        public string StopSequenceDescriptionBuilder(List<Station> stopSequence)
        {
            if (stopSequence.Count == 2)
            {
                return $"This train stops at {string.Join(" and ", stopSequence.Select(s => s.Name))} only";
            }
            else if (stopSequence.Count > 2)
            {
                // Check if all the stations in a stop sequence are stopping stations
                if (stopSequence.All(s => s.IsStopping))
                {
                    return "This train stops at all stations";
                }
                else
                {
                    // List of express stations in a stop sequence
                    List<Station> expressStations = stopSequence.Where(s => !s.IsStopping).ToList();

                    if (expressStations.Any())
                    {
                        // Generate express sections in a stop sequence
                        var expressSections = GenerateExpressSections(stopSequence, expressStations);
                        // Returns express sections description for a stop sequence
                        return GenerateExpressSectionDescription(stopSequence, expressSections);
                    }

                    return "Unable to determine the stopping sequence";
                }
                             
            }

            return "Unable to determine the stopping sequence";            
        }

        public List<StopSequenceExpressSection> GenerateExpressSections(List<Station> stopSequence, List<Station> expressStations)
        {
            // Next express station's index in the express stations list
            int nextExpressStationIndex = 0;
            // Current express station's index in the express stations list
            int currExpressStationIndex = 0;
            // Current express station's index in the stop sequence
            int currExpressStationIndexInStopSequence = 0;
            // Next express station's index in the stop sequence
            int nextExpressStationIndexInStopSequence = 0;

            List<StopSequenceExpressSection> sequenceSections = new List<StopSequenceExpressSection>();

            // First station of stop sequence's express section one
            Station sectionOneFirstStation = stopSequence.First();
            // Second station of stop sequence's express section two
            Station sectionOneLastStation = stopSequence.Last();
            // First station of stop sequence's express section two
            Station sectionTwoFirstStation = stopSequence.First();
            // Second station of stop sequence's express section two
            Station sectionTwoLastStation = stopSequence.Last();

            foreach (Station station in expressStations)
            {
                currExpressStationIndex = expressStations.IndexOf(station);

                if(currExpressStationIndex == expressStations.Count - 1)
                {
                    break;
                }

                nextExpressStationIndex = currExpressStationIndex + 1;

                // Find and store current the express station's index from stop sequence
                currExpressStationIndexInStopSequence = stopSequence.IndexOf(station);
                // Find and store the next express station's index from stop sequence
                nextExpressStationIndexInStopSequence = stopSequence.IndexOf(expressStations[nextExpressStationIndex]);

                /*
                 * This block checks if the difference between the next and current express station index is greater than
                 * or less than 3. This way it separates all the express sections in the stop sequence.
                 */
                if (nextExpressStationIndexInStopSequence - currExpressStationIndexInStopSequence >= 3)
                {
                    /* 
                     * Store the last stopping station of the first express section. This will always be the next station
                     * to the current express station in a stop sequence.
                     */
                    sectionOneLastStation = stopSequence[currExpressStationIndexInStopSequence + 1];

                    /* 
                     * Store the first stopping station of the second express section. This will always be the second station
                     * from the current express station in a stop sequence.
                     */
                    sectionTwoFirstStation = stopSequence[currExpressStationIndexInStopSequence + 2];

                    var sectionToAdd = new StopSequenceExpressSection(sectionOneFirstStation, sectionOneLastStation);

                    // Add the new express section to the stop sequence express sections list
                    sequenceSections.Add(sectionToAdd);

                    // Assign the second express section's first station to the first express section's first station
                    sectionOneFirstStation = sectionTwoFirstStation;
                } 
            }

            // Add final express section of the remaining stop sequence
            var finalStopSequenceExpressSection = new StopSequenceExpressSection(sectionOneFirstStation, sectionTwoLastStation);
            sequenceSections.Add(finalStopSequenceExpressSection);

            return sequenceSections;
        }

        private string GenerateExpressSectionDescription(List<Station> stopSequence, List<StopSequenceExpressSection> expressSections)
        {
            // List of stop sequence sections to store stations between express section's first and last stations
            List<Station> stopSequenceSections = new List<Station>();

            // List of description strings
            List<string> description = new List<string>();

            description.Add("This train");

            foreach (StopSequenceExpressSection expressSection in expressSections)
            {
                int rangeFirstIndex = stopSequence.IndexOf(expressSection.FirstStation) + 1;
                int rangeLastIndex = stopSequence.IndexOf(expressSection.LastStation);

                // Stop sequence stations between Express section's first and last stations
                 stopSequenceSections = stopSequence.Skip(rangeFirstIndex).Take(rangeLastIndex - rangeFirstIndex).ToList();
                
                // Check if the stop sequence section has any express stations
                if (stopSequenceSections.Any(station => !station.IsStopping))
                {
                    // Check if the stop sequence section has only one express station
                    if (stopSequenceSections.Where(ss => !ss.IsStopping).Count() == 1)
                    {
                        description.Add($" stops at all stations except { stopSequenceSections.Where(s => !s.IsStopping).First().Name }");
                    }
                    else
                    {
                        description.Add($" runs express from { expressSection.FirstStation.Name} to {expressSection.LastStation.Name }");
                        
                        // Check if the stop sequence section has only one stopping station
                        if (stopSequenceSections.Count(station => station.IsStopping) == 1)
                        {
                            Station stoppingStation = stopSequenceSections.Where(s => s.IsStopping).First();
                            description.Add($", stopping only at {stoppingStation.Name}");
                        }
                    }                   

                    // Check if there are more express sections
                    if (expressSection != expressSections[expressSections.Count() - 1])
                    {
                        description.Add(" then");
                    }                    
                }

                // Clear stop sequence section after each express section
                stopSequenceSections.Clear();
            }

            return string.Join("", description);
        }
    }
}
