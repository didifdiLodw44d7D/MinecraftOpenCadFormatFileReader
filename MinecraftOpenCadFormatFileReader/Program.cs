using MinecraftOpenCadFormatFileReader;
using System.Net;
using System.Net.Security;

var ocaReader = new OCAReader(@"Path-oca_file", new CadPoint(1000, 1000, -59), "minecraft:stone");

var ipAddress = IPAddress.Parse("SERVER IP ADDRESS");
ushort port = ushort.Parse("SERVER POSRT");
var password = "fujimaki";
//var command = "setblock ~ -55 ~ minecraft:gold_block";
var connection = new CoreRCON.RCON(ipAddress, port, password);

await connection.ConnectAsync();

//foreach (var cmd in mcmd.structureCommands)
foreach (var oca in ocaReader.cmd)
{
    // Send "status" and try to parse the response
    string respnose = await connection.SendCommandAsync(oca);
}

foreach (var oca in ocaReader.cmd)
{
    Console.WriteLine(oca);
}