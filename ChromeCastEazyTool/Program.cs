using ChromeCastEazyTool;
using GoogleCast;
using GoogleCast.Channels;
using GoogleCast.Models.Media;
using Newtonsoft.Json;

const string settingPath = "Setting.json";
if (File.Exists(settingPath) is false)
{
	Console.WriteLine($"Cannot find {settingPath}");
	return;
}

var text = File.ReadAllText("Setting.json");
if (string.IsNullOrEmpty(text))
{
	Console.WriteLine("Setting content is empty");
	return;
}

Setting? setting;
try
{
	setting = JsonConvert.DeserializeObject<Setting>(text);
}
catch
{
	Console.WriteLine("Cannot deserialize setting.json");
	return;
}

var deviceName = setting?.DeviceName;
if (string.IsNullOrEmpty(deviceName))
{
	Console.WriteLine("deviceName is empty");
	return;
}

var videoUrl = setting?.VideoUrl;
if (string.IsNullOrEmpty(videoUrl))
{
	Console.WriteLine("videoUrl is empty");
	return;
}

var deviceLocator = new DeviceLocator();

var recievers = await deviceLocator.FindReceiversAsync();

var receiver = recievers.FirstOrDefault(z => string.Equals(z.FriendlyName, deviceName, StringComparison.OrdinalIgnoreCase));

if (receiver == null)
{
	Console.WriteLine("reciever is not finded");
	return;
}

var sender = new Sender();

await sender.ConnectAsync(receiver);

var mediaChannel = sender.GetChannel<IMediaChannel>();

await sender.LaunchAsync(mediaChannel);

var mediainfo = new MediaInformation
{
	ContentId = videoUrl
};

var mediaStatus = await mediaChannel.LoadAsync(mediainfo);

Console.WriteLine("Press any key to exit");
Console.ReadKey();