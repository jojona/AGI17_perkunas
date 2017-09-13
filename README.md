# AGI17_perkunas

This project was created for the 2017 edition of the course Advanced Computer Graphics and Interaction at KTH.
Perkunas is a Virtual Reality game developed in Unity.

# Important Note for Linux Users

Steam VR may make Unity crash when opening the project/scenes. If this happens, open the file openvr_api.cs and add the following lines at the beginning of the OpenVR.Init() function (it is a static function around line 4967) :

```
#if UNITY_EDITOR_LINUX
return null;
#endif
```

It may cause some error messages in the Unity console but you will be able to open and work on the project.
