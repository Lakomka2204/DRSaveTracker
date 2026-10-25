using System.Reflection;

namespace DRSTCore
{
    public class RoomMapper
    {
        public RoomMapper()
        {
            roomDict = [];
            roomDict[0] = new string('-',12);
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream("DRSTCore.Assets.rooms.csv");
            if (stream is null)
            {
                Console.WriteLine("Asset not found, will be using numbers");
                return;
            }
            using StreamReader reader = new(stream);
            reader.ReadLine();
            string? line;
            while((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var fields = line.Split(',');
                if (fields.Length < 4)
                {
                    Console.WriteLine("Warn: line has less than 4 elements, line: {0}",line);
                    continue;
                }
                int id = int.Parse(fields[0]);
                string roomName = fields[3];
                roomDict[id] = roomName;
            }
        }
        ~RoomMapper()
        {
            roomDict.Clear();
        }
        private readonly Dictionary<int, string> roomDict;

        public string? this[int id]
        {
            get
            {
                if (roomDict.TryGetValue(id, out var room))
                    return room;
                else
                    return null;
            }
        }
    }
}
