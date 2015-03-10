# LptIo

A simple Windows Console tool to control the PC's LPT ports.

## Requirements

### .NET Framework

  * __Version__: 4.x
  * __Website__: http://www.microsoft.com/net
  * __Download__: http://www.microsoft.com/net/downloads

## Usage

Run the executable `LtpIo.exe` and write the `READ` or `WRITE` commands to the standard input.

### READ

Request: `READ <lpt port to read> <bit to read>`
Response: `READ <read lpt port> <read bit> <read value> <read state>`

### Write

Request: `WRITE <lpt port to write> <bit to write> <state to set>`
Response: `WRITE <written lpt port> <written bit> <set nstate> <written value>`

## License

This project is released under the [MIT License](https://raw.github.com/morkai/LptIo/master/license.md).

The `inpout32.dll` and `inpoutx64.dll` are copyrighted by [http://www.highrez.co.uk/Downloads/InpOut32/](Logix4U & Phillip Gibbons [Highresolution Enterprises] (for the x64 port)).
