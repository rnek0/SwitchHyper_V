# SwitchHyper_V 

Application that helps to switch on/off Hyper-V.

![Alt Screenshot](./app.png?raw=true "SwitchHyper_V")

[Doc Hyper-V](https://docs.microsoft.com/fr-fr/virtualization/hyper-v-on-windows/quick-start/enable-hyper-v)

---

Another way(no need to make an application in C # except for fun) :

To __disable Hyper-V__ to use VirtualBox, open a command prompt as an administrator and run the following command:

```cmd
bcdedit /set hypervisorlaunchtype off
```

You will need to reboot, but then you will be ready to run VirtualBox. 

To __reactivate Hyper-V__, run:

```cmd
bcdedit /set hypervisorlaunchtype auto
```

and reboot.

---

Do not forget to stop the boot of Docker Desktop when booting the pc, otherwise it will alert you to the absence of Hyper-V.

After reactivating Hyper-v you will be able to reuse Docker Desktop.


For another use:

![Alt HAXM install](./Hyper-V.png?raw=true "HAXM")

After use Switch Hyper V

![Alt HAXM install](./Hyper-V_off.png?raw=true "HAXM")