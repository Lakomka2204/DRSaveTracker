namespace DRSTCore
{
    public class RoomMapper
    {
        public string? CacheDirectory { get; set; }
        public RoomMapper()
        {
            client = new HttpClient();
            lightWorldDict = [];
            darkWorldDict = [];
        }
        public RoomMapper(string cacheDir) : this()
        {
            CacheDirectory = cacheDir;
        }
        private async Task WriteToCache(string filename, string content)
        {
            if (CacheDirectory == null)
                return;
            if (!Directory.Exists(CacheDirectory))
                Directory.CreateDirectory(CacheDirectory);
            var fullName = Path.Join(CacheDirectory, filename);
            using StreamWriter sw = new(fullName);
            sw.Write(content);
        }
        private async Task<string?> GetFromCache(string filename)
        {
            var fullName = Path.Join(CacheDirectory, filename);
            if (!File.Exists(fullName))
                return null;
            FileInfo fi = new(fullName);
            var age = fi.LastWriteTime - DateTime.Now;
            if (age.TotalDays > 30)
                return null;
            using StreamReader sr = new(fi.OpenRead());
            return await sr.ReadToEndAsync();
        }
        public void UnloadRoomInfo()
        {
            lightWorldDict.Clear();
            darkWorldDict.Clear();
        }
        public async Task LoadRoomInfo()
        {
            for (int i = 1; i <= MAX_CHAPTERS; i++)
            {
                var cUrl = string.Format(URL, i);
                var filename = Path.GetFileName(cUrl);
                var csv = await GetFromCache(filename);
                if (csv == null)
                {
                    var response = await client.GetAsync(cUrl);
                    if (!response.IsSuccessStatusCode) continue;
                    csv = await response.Content.ReadAsStringAsync();
                    await WriteToCache(filename, csv);
                }
                using var reader = new StringReader(csv);
                await reader.ReadLineAsync();
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var fields = line.Split(',');

                    int lwId = int.Parse(fields[0]);
                    int dwId = int.Parse(fields[1]);
                    string roomName = fields[2];

                    lightWorldDict[lwId] = roomName;
                    if (!darkWorldDict.ContainsKey(dwId))
                        darkWorldDict[dwId] = roomName;
                }
            }
        }
        private readonly HttpClient client;
        public const string URL = "https://raw.githubusercontent.com/portaldash/deltarooms/refs/heads/main/chapter{0}_output.csv";
        public const int MAX_CHAPTERS = 5;
        private readonly Dictionary<int, string> lightWorldDict;
        private readonly Dictionary<int, string> darkWorldDict;

        public string? this[int id]
        {
            get
            {
                if (id > 1e4)
                {
                    if (darkWorldDict.TryGetValue(id, out var room))
                        return room;
                    else
                        return null;
                }
                else
                {
                    if (lightWorldDict.TryGetValue(id, out var room))
                        return room;
                    else
                        return null;
                }
            }
        }
    }
}
