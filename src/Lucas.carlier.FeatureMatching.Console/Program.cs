// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Lucas.carlier.FeatureMatching;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please provide the object image path and the scenes directory path.");
            return;
        }

        var objectImagePath = args[0];
        var scenesDirectoryPath = args[1];

        if (!File.Exists(objectImagePath))
        {
            Console.WriteLine($"The object image file '{objectImagePath}' does not exist.");
            return;
        }

        if (!Directory.Exists(scenesDirectoryPath))
        {
            Console.WriteLine($"The scenes directory '{scenesDirectoryPath}' does not exist.");
            return;
        }

        var objectImageData = await File.ReadAllBytesAsync(objectImagePath);
        var imagesSceneData = (await Task.WhenAll(Directory.EnumerateFiles(scenesDirectoryPath)
            .Select(async filePath => await File.ReadAllBytesAsync(filePath)))).ToList();

        var objectDetection = new ObjectDetection();
        var detectObjectInScenesResults =
            await objectDetection.DetectObjectInScenesAsync(objectImageData, imagesSceneData);

        PrintObjectDetectionResults(detectObjectInScenesResults);
    }

    static void PrintObjectDetectionResults(IList<ObjectDetectionResult> detectObjectInScenesResults)
    {
        foreach (var objectDetectionResult in detectObjectInScenesResults)
        {
            Console.WriteLine($"Points: {JsonSerializer.Serialize(objectDetectionResult.Points)}");
        }
    }
}

