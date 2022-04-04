using ChromeCastEazyTool;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Newtonsoft.Json;

var text = File.ReadAllText("Setting.json");
var setting = JsonConvert.DeserializeObject<Setting>(text);
if (setting == null)
{
	Console.WriteLine("Cannot find setting.json");
	return;
}

var deviceLocator = new DeviceLocator();

var recievers = await deviceLocator.FindReceiversAsync();

var receiver = recievers.FirstOrDefault(z => string.Equals(z.FriendlyName, setting!.DeviceName, StringComparison.OrdinalIgnoreCase));

if (receiver == null)
{
	Console.WriteLine($"Invalid device name - {setting!.DeviceName}");
	return;
}

var sender = new Sender();

await sender.ConnectAsync(receiver);

var mediaChannel = sender.GetChannel<IMediaChannel>();

await sender.LaunchAsync(mediaChannel);

var mediainfo = new MediaInformation
{
	ContentId = setting!.VideoUrl
};

var mediaStatus = await mediaChannel.LoadAsync(mediainfo);

Console.WriteLine("Press any key to exit");
Console.ReadKey();