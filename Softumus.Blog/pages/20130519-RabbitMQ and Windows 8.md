Recently I had a strange error on one of customer's laptop. We use Rabbit MQ as a message queue in our application, and it didn't want to start on that laptop. Main idea for now is that it's because of Windows 8 on that laptop, but maybe it's just coincidence. So, it was preamble, and now let's go to real story...

Oh, small sidenote - it's a story about Erlang v14 and RabbitMQ v2.8.1. Maybe, in latest versions everything is fixed already.

So, RabbitMQ's service didn't want to start. In log (they are in c:\Users\<your_user>\AppData\Roaming\RabbitMQ\log\, more detailed about RabbitMQ's folders you can read here) we didn't find anything readable except something like "there is an exception when I try to open port". So, first idea was that somebody holds one of necessary ports (they are 4369 for Erlang and 5672+55672 for RabbitMQ. Latter one changed to 15672 in RabbitMQ v3.0). But real problem was in filesystem.

I don't know if it's new defaults for Windows 8, but C: volume has disabled short names (8.3) for files/folders. As result - RabbitMQ tried to start some application to check if port is free or something like that, and didn't manage to do it. You can check if this is your case by this command (type it in command line):

	fsutil behavior query disable8dot3 c:

If value is not 0 (8dot3 name creation is disabled) - it's most probably that you have the same problem. In this case you should first - enable creation of short names:

	fsutil behavior set disable8dot3 c: 0

Maybe it's even better to enable it for all volumes (in this case just skip "c:" param in line above).
Bad news is that short names will not re-created automatically. And we need short names for all parts of path to RabbitMQ/Erlang (e.g., even for "Program Files"). Theoretically, you can try do the following:

	fsutil file setshortname "c:\Program Files\" PROGRA~1

But I had "access denied" without any success to resolve it. So, workaround in this case is to uninstall Erlang + RabbitMQ and then install them in new folder (e.g. - to c:\tools\). Be sure, that ALL parts of path to RabbitMQ + Erlang has short names (you can check it with for example "dir /X" command).

That's it. I hope you have running RabbitMQ now.