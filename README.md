Xenophyte-Remote-Node
Xenophyte Remote Node version 0.2.7.8R compatible with Windows with Netframework 4.6.1 or higher or other OS like Linux who need to use Mono.
In production, we suggest to compile source in Release Mode to disable log files and debug mode.

Please check our wiki for get help about the remote node tool:
https://github.com/XENOPHYTE/Xenophyte-Remote-Node/wiki

Supported Firewall on the API Ban system
-> PF [FreeBSD, OpenBSD and others BSD who support PacketFilter]

-> iptables [Linux usually]

-> Windows Firewall (Automaticaly used once the tool is launched on Windows)

Installation Instructions
On Linux OS (Work also Raspbian OS for Raspberry):

sudo wget https://github.com/XENOPHYTE/Xenophyte-Remote-Node/releases/download/0.2.7.8R/Xenophyte-RemoteNode-0.2.7.8R-Linux-64bit.zip
or:

sudo wget https://github.com/XENOPHYTE/Xenophyte-Remote-Node/releases/download/0.2.7.8R/Xenophyte-RemoteNode-0.2.7.8R-Raspberry.zip

sudo unzip Xenophyte-RemoteNode-0.2.7.8R-Linux.zip

or:

sudo unzip Xenophyte-RemoteNode-0.2.7.8R-Raspberry.zip

sudo chmod 0777 Xenophyte-RemoteNode-Linux or Xenophyte-RemoteNode-Raspberry

sudo ./Xenophyte-RemoteNode-Linux or sudo ./Xenophyte-RemoteNode-Raspberry

Newtonsoft.Json library is used since version 0.2.2.8b for the API HTTP/HTTPS system: https://github.com/JamesNK/Newtonsoft.Json

Developers:

Xenophyte 
