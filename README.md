# Kinect_Worktable
We develop a mechatronic control system based on Kinect and 3-DOF worktable. The system obtains the real-time positions of human key points from Kinect, and then records the trajectory of human hands. The trajectory can be converted to the G code file (the command in numerically controlled machine) through CAM software. To make it understandable, our plug-in component interprets the G code and guides the 3-DOF worktable to draw the same trajectory.

## KincectBodyBasics
Functions: The program is the upper system connected to Kinect device and the lower system. The Kinect records the real-time hands positions. The upper system then processes the position data and encodes it to the lower system through serial communication. Meanwhile, the upper system obtains the feedback data from the lower system continously. The decoder module works to retrieve understandable information.  

Components
- EncoderDecoder: The implementation of the encoder and decoder for data tranlation between the upper and lower system.
- IEncoder: The interface of the encoder and decoder.
- SerialControl: The serial communication between the upper and lower system.
- IPublicPlugInInterface: Encapsulate the interface of plug-in components. Any plug-in modules inheriting this interface can be called by this program.
- HandControl: The interaction logic between human hands and the upper system.

## Worktable
Functions: Another upper system which integrates several plug-in components and provides debugging interface. The functions include: 
- [x] Draw and record an arbitrary trajectory.
- [x] Linear and circular interplation.
- [x] G code Interpreter.
- [x] Serial-communication.
- [x] Pointing control by mouse.

Interface:
<div align=center>
<img src='Images/interface.jpg' width='800'>
</div>

Components
- EncoderDecoder: The implementation of the encoder and decoder for data tranlation between the upper and lower system.
- IEncoder: The interface of the encoder and decoder.
- IPublicPlugInInterface: Encapsulate the interface of plug-in components. Any plug-in modules inheriting this interface can be called by this program.
- GCodeInterpreter: The interpreter of the generated G code, controling the lower part motion.
- lnterpolation: The linear interplation program.
- LinerInter: The circular interplation program.
- PlugInDrawCopy: A plug-in component that users can draw an arbitrary trajectory and the system will complete interplation.
- PlugInDrawGraphic: A plug-in component that shows the interplated trajectory.
- ResLib: Resources.
- ThreeCoordinateMainWindow: The main display window.



