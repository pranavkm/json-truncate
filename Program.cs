using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "repro.json"));
            var objects = JsonSerializer.Deserialize<List<SessionResponse>>(input);

            // This works fine.
            var json = JsonSerializer.Serialize(objects);
            Console.WriteLine(input == json);

            // This does not
            var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, objects);
            var jsonAsync = Encoding.UTF8.GetString(memoryStream.ToArray());

            Console.WriteLine(jsonAsync == json);
            Console.WriteLine($"{jsonAsync.Length} == {json.Length}");
        }
    }

    public class Session
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual string Abstract { get; set; }

        public virtual DateTimeOffset? StartTime { get; set; }

        public virtual DateTimeOffset? EndTime { get; set; }

        // Bonus points to those who can figure out why this is written this way
        public TimeSpan Duration => EndTime?.Subtract(StartTime ?? EndTime ?? DateTimeOffset.MinValue) ?? TimeSpan.Zero;

        public int? TrackId { get; set; }
    }

    public class SessionResponse : Session
    {
        public Track Track { get; set; }

        public List<Speaker> Speakers { get; set; } = new List<Speaker>();
    }

    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Speaker
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public virtual string WebSite { get; set; }
    }
}
