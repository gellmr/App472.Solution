# App472.Solution #

This is an .NET MVC5 application made with Visual Studio.

Below are steps to build and deploy the app to Amazon EC2.

RELATED FILES (not included in the repo)

- path/to/repos/App472.Syd.Dev/Keys           (App472.Syd.Key.pem)
- path/to/repos/App472.Syd.Dev/Remote Desktop (App472.Syd.Dev.rdp)

- path/to/repos/App472.Syd.Dev/secrets.release  (user secrets for production)
- path/to/repos/App472.Syd.Dev/secrets.debug    (user secrets for development)
- path/to/repos/App472.Syd.Dev/secrets.use      (selected version)

---------------------------------------------------------------

## HOW TO CREATE EC2 INSTANCE WITH WINDOWS SERVER TO HOST THE APP ##

### Settings for EC2 Instance: ###

Navigate to create a new instance...

`Asia Pacific Sydney` -> `EC2` -> `Instances` -> `Launch an instance`

Name: `App472.Syd.Dev`

Microsoft Windows Server 2022 Base

64-bit (x86)

Microsoft Windows Server 2025 Full Locale English AMI provided by Amazon

Instance Type
`t2.small`
(about $150 for 6 months)

key pair:
`App472.Syd.Key`
(pem)

storage
`30 GB gp2 SSD`

Launch Instance

--------------------------------------------------

## CONNECT TO THE INSTANCE: ##

From EC2 instances, right click the `App472.Syd.Dev` instance and choose Connect

Choose RDP client

If you havent already, click "Download remote desktop file"

This gives you `App472.Syd.Dev.rdp` (the RDP client file)

I have stored it in...

`path/to/repos/App472.Syd.Dev/Remote Desktop/App472.Syd.Dev.rdp`

Note that you need to re-download the rdp file every time you restart the instance.

On this page you will also see "Get password" at the bottom. Click this link.

It says "Get Windows password" and asks you to "Upload private key file"

Upload the `App472.Syd.Key` (pem) key file

It will show the decrypted key contents "BEGIN RSA PRIVATE KEY ..."

Ignore this and click "Decrypt password"

You will see the plain text password. It will be scrambled numbers, letters, and symbols about 32 characters long...

################################

Copy this plain text `password` to your clipboard ... we are about to use it with the (downloaded) `App472.Syd.Dev.rdp` file.

Double click `App472.Syd.Dev.rdp` ... this will launch `Remote Desktop Connection` which is a windows program.

Important - Right click to pin the program to your taskbar.

Now Right click again to open a context menu, and launch `Remote Desktop Connection` again...

At the bottom corner of the program is an expanding menu called `Show Options` ... Click this.

Under `Connection settings` choose `Open` and select the .rdp file we were just using.

This will populate the Computer and User name fields. Now click the tab `Local Resources`

Under `Local devices and resources` click `More`

Under `Drives` click `Windows C:` and OK

This is super important as we need to be able to access our local filesystem to send the deploy files to our remote instance.

Go ahead and click `Connect` ... there is a warning that it cannot identify the remote computer. Go ahead and click `Connect` to proceed.

Next appears a window saying `Enter your credentials`

Paste the `password` we got earlier.

There is one more warning that the certificate is not trusted. Proceed anyway (Yes)

You should see the blue desktop of the remote windows server instance.

Now you are logged in, you can go through windows settings and change the `password` to something else if you want.

This will help if you go away from keyboard and come back to find the rdp has logged out and wants you to type the long password again.

NOTE - if you stop the instance... `App472.Syd.Dev.rdp` will become invalid.
You will need to delete `App472.Syd.Dev.rdp` and repeat the above steps again. (CONNECT TO THE INSTANCE)

--------------------------------------------------

## HOW TO Install IIS, on Windows Server ##

After you Remote Desktop in, to the server instance, you should see the blue desktop.

Go to Start and type "Server Manager"

You should see the server manager dashboard.

Click "Add roles and features"

You should see "Before you begin..." click Next

You should see "Select installation type" click "Role-based/feature-based installation"

You should see "Select destination server"

We are going to run the server on the machine itself (local)...

So choose the first option... "Select a server from the server pool"

The server pool is the table below. It shows the name, ip address and OS, of our current (local) machine.

Click Next.

You should see "Select server roles"

This page has a long list of Roles, some are already installed on the server (such as File and Storage Services)

Dont remove anything.

Click the square to add "Web Server (IIS)"

A dialog will appear, saying you need certain tools to manage the server. If you were setting up many servers and you wanted to manage them all remotely from some other instance, you would only need to install the tools there. But since we have just one server and we are setting it up now... we want to install the tools. Leave the checkbox ticked and click "Add Features"

It should return to the previous page, (Server Roles).

We are done adding Roles. Click Next.

You should see "Select features"

It should have ".NET Framework 4.8 Features" already checked. Leave this alone.

(NOT SURE IF I NEED EXTRA FEATURES)
(NOT SURE IF I NEED EXTRA FEATURES)
(NOT SURE IF I NEED EXTRA FEATURES)

We dont need to add anything, so click "Next"

You should see "Web Server Role (IIS)" and some information. Click "Next"

You should see "Select role services"

There are many items you can install. The defaults are currently selected.

You dont have to worry about this now, because its possible to come back later and install more stuff. So just leave the defaults selected, and click "Next"

You should see "Confirm installation selections".

There is no need to restart the server, when we are installing IIS. It will be fine.

Click "Install" ...It will take about 2 minutes.

The progress bar will say "Installation succeeded" and you can click "Close"

While still Within the remote windows server, open up Edge browser (it should be installed) and navigate to localhost:80

You should see the Internet Information Services landing page with blue squares, so we know IIS is working on the local machine.

If you exit the RDP client and try to navigate to the ipv4 address and port 80, you will probably not see anything... as we have not configured port settings on Amazon EC2.

--------------------------------------------------

## HOW TO Install SQL Server 2022 Express, on the windows server ##

Within the RDP client, on the windows server, open Edge browser and navigate to...
https://www.microsoft.com/en-us/download/details.aspx?id=104781

(or search for "sql server 2022 express" and find the official download. The above link is an official download.)

You will see a microsoft page. Click "Download"

From within the Downloads folder, double click the downloaded installer.

The installer comes up with a dark grey window. Choose "Basic" installation.

Accept the software license agreement.

Default install location is...
`C:\Program Files\Microsoft SQL Server`

Click "Install"

It takes a few minutes to install.

When the install completes, it will show...

The instance name, connection string, and some other information.

(Keep a copy of this connection string... we will need it later.)

Click "Close"

--------------------------------------------------

## INSTALL NVM ##

On your local development machine, you will need nvm to manage different node installations.

Download the Windows version from the following URL...

https://github.com/coreybutler/nvm-windows/releases/download/1.1.12/nvm-setup.exe

Run the above installer to install nvm on your workstation.

After installing nvm, open powershell as Administrator and type...
`nvm -v`
1.1.12

NOTE - I had to choose older packages, around 2018-2020, to get this project working.

`nvm install 8.16.1`

`nvm use 8.16.1`

`nvm list`
-    10.5.0
-    `8.16.1` (Currently using 64-bit executable)
-    6.17.1
-    6.11.2
-    Shows different node versions, installed under nvm

`node -v`
8.16.1

`npm -v`
6.4.1

--------------------------------------------------

## Use npm to Install dependencies to node_modules ##

Run the following command to ensure our node packages are up to date...
npm install

This will take a few minutes to install lots of things under

`path/to/repos/App472.Solution/node_modules`

--------------------------------------------------

## Building the MVC5 application ##

In visual studio, ensure you are in Release mode.

When we build the solution Visual studio places compiled dll files from the App472.Domain project into the App472.WebUI project.

So we only need to build and deploy the App472.WebUI project.

We dont need to deploy the `Domain` project or `Test` project.

Clean and Build the project. Choose Build -> Rebuild Solution

It will produce files in the `...App472.Solution/App472.WebUI/bin` folder

Now, we need to choose `Build -> Publish App472.WebUI`

Ensure the settings say:

Target location: `../Deploy.to.wwwroot/`
Delete existing files: `true`
Configuration: `Release`

Under `Show all settings` ... 
	`YES` Delete all existing files prior to publish
	`NO`  Precompile during publishing
	`YES` Exclude files from the App_Data folder

Click `Publish`

This will generate files in the `Deploy.to.wwwroot` folder.

--------------------------------------------------

## Manually place the build files into IIS web root folder ##

Login to EC2...

https://ap-southeast-2.console.aws.amazon.com/console/home?region=ap-southeast-2#

Go to EC2 instances and ensure `App472.Syd.Dev` instance is running.

If you havent already, follow the steps above to (CONNECT TO THE INSTANCE)

You should see the blue desktop.

Launch "Server Manager"

Under "All Servers" you should see the IIS instance we created earlier. Its name will be like "EC2AMAZ-3SE3CO2" or similar.

Right-Click this item and choose "Internet Information Services (IIS) Manager" from the context menu.

From the "Connections" list on the side, expand down until you see "Default Web Site"

- Start Page
- EC2AMAZ-3SE3CO2 (EC2AMAZ-3SE3CO2\Administrator)
-  *  Application Pools
-  *  Sites
-  *  -  Default Web Site     <-- explore to here

You will see "Actions" on the right hand side of the screen and an icon saying "Explore". Click Explore.

It will open a file explorer window, at the location `C:\inetpub\wwwroot`

This is our root directory for serving html pages.

There should just be 2 files in the wwwroot folder...
	iisstart.htm
	iisstart.png

Delete these files.

Since (earlier) we gave `Remote Desktop Connection` access to our `Windows C:`

you should see `C on DESKTOP-S602TTD` or similar, in File Explorer on the left hand side under `This PC`

Click this to view the C: of your local workstation (BE CAREFUL!)

You can browse to the location of `Deploy.to.wwwroot`

It should look something like `\\tsclient\C\Users\YourUserName\source\repos\App472.Solution\Deploy.to.wwwroot`

The contents should be like this:
	bin
	Content
	Scripts
	Views
	favicon.ico
	Global.asax
	Web.config

Copy and past all these into the `C:\inetpub\wwwroot` folder

--------------------------------------------------
