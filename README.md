ConradUisControl
================

Tool to control Conrad's UIS manually and make it usable programmatically.

Establishes a webserver on the local machine so that it can be used by humans and other applications to control the outlets.

Usage
=====

Find the 'ConradUisControl.exe.config' file in your bin-directory. Open it with any text editor.
You then find this content:

  <appSettings>
    <add key="listenPort" value="5112" />
    <add key="deviceIp" value="192.168.178.112" />
    <add key="devicePort" value="5112" />
    <add key="username" value="" />
    <add key="password" value="" />
  </appSettings>

Descriptions:

- 'listenPort': the port on the local machine where the webserver shall wait for GET requests
- 'deviceIp': the IP of the device within your network
- 'devicePort': the port of the device
- 'username': used for authentication on the device
- 'password': used for authentication on the device
