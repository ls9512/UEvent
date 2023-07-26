# UEvent 

**UEvent** is a general message event component that can be used in unity and native .Net environment. It can provide powerful decoupling capability through event mechanism.

![license](https://img.shields.io/github/license/ls9512/UEvent)
[![openupm](https://img.shields.io/npm/v/com.ls9512.uevent?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.ls9512.uevent/)
![topLanguage](https://img.shields.io/github/languages/top/ls9512/UEvent)
![size](https://img.shields.io/github/languages/code-size/ls9512/UEvent)
![issue](https://img.shields.io/github/issues/ls9512/UEvent)
![last](https://img.shields.io/github/last-commit/ls9512/UEvent)
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)

[[中文文档]](README_CN.md)

> Official QQ Group：[1070645638](https://jq.qq.com/?_wv=1027&k=ezkLnUln)

<!-- TOC -->

- [UEvent](#uevent)
	- [1 Introduction](#1-introduction)
		- [1.1 Environment](#11-environment)
		- [1.2 Folder](#12-folder)
		- [1.3 Feature](#13-feature)
		- [1.4 Structure](#14-structure)
		- [1.5 Rules / Recommendations](#15-rules--recommendations)
		- [1.6 Start](#16-start)
	- [2 Component](#2-component)
		- [2.1 Event Manager](#21-event-manager)
		- [2.2 Event Dispatcher](#22-event-dispatcher)
		- [2.3 Event Listener](#23-event-listener)
		- [2.4 Object  Listener](#24-object--listener)
		- [2.5 Unity MonoListener](#25-unity-monolistener)
		- [2.6 Event Handler](#26-event-handler)
			- [2.6.1 EventHandler](#261-eventhandler)
			- [2.6.2 EventHandler\<T\>](#262-eventhandlert)
	- [3 Optional Attribute](#3-optional-attribute)
		- [3.1 EventEnumAttribute](#31-eventenumattribute)
		- [3.2 ListenAttribute](#32-listenattribute)
		- [3.3 ListenTypeAttribute](#33-listentypeattribute)
		- [3.4 ListenGroupAttribute](#34-listengroupattribute)
	- [4 API](#4-api)
		- [4.1 Event Definition](#41-event-definition)
		- [4.2 Auto Listener](#42-auto-listener)
		- [4.3 Manual Listener](#43-manual-listener)
		- [4.4 Listen Event](#44-listen-event)
		- [4.5 Listen Event Type](#45-listen-event-type)
		- [4.6 Listen Group](#46-listen-group)
		- [4.7 Maunal Register / Deregister Event](#47-maunal-register--deregister-event)
		- [4.8 Dispatch Event](#48-dispatch-event)
		- [4.9 Dispatch Event (Thread Safety for Unity)](#49-dispatch-event-thread-safety-for-unity)
		- [4.10 Dispatch to Target](#410-dispatch-to-target)
		- [4.11 Dispatch to Group](#411-dispatch-to-group)
		- [4.12 Full API](#412-full-api)
	- [5 Compatibility Features](#5-compatibility-features)
		- [5.1 Performance Optimization](#51-performance-optimization)
			- [5.1.1 Preloading](#511-preloading)
		- [5.2 Compatibility List](#52-compatibility-list)
		- [5.3 \>string Event](#53-string-event)
		- [5.4 class / struct Event](#54-class--struct-event)

<!-- /TOC -->

## 1 Introduction
### 1.1 Environment
![Unity: 2019.4.3f1](https://img.shields.io/badge/Unity-2017+-blue) 
![.NET 4.x](https://img.shields.io/badge/.NET-4.x-blue) 

### 1.2 Folder
* **Samples**: Example folder. It can be deleted in the you project to reduce space consumption.
* **CSharp**: The core fully implemented by .Net, can be used independently in .Net environment.
* **Unity**:  The additional functions implemented by unity class library, you need to work with the code in the core folder when working in unity environment.

### 1.3 Feature
* Support to define multiple groups of events through enumeration, and monitor by single event or by event type.
* At the same time, it supports enum / string / class / struct type event definition to minimize changes and be compatible with different projects.
* Support receiving event priority.
* Support event grouping or sending for specific target objects.
* Support specific listener to interrupt the entire listen event queue.
* It also supports common methods and delegated methods.
* Provide the `ObjectListener` and `MonoListener` base classes, so that any class can automatically register/remove listeners, and you can also implement the `IEventListener` interface yourself.

### 1.4 Structure
* **EventManager** -> **EventDispatcher**-> **EventHandler**
* Internal implementation : **EventListener** -> **EventDispatcher**
* External implementation : **UserEventListener**

### 1.5 Rules / Recommendations
* For each type of event, use an enumerated type such as AppEvent, GameEvent, etc. Each event type corresponds to an event dispatcher instance.
* It is possible to use only one event type per project, but it is not recommended.
* Method-type listener needs to specify the binding object, while delegate-type listener does not need to specify the object.
* For the listen method of the delegate type, if there are parameters, the parameters use the general **object[]** form, so **eventType** may need to be specified for processing.
* It is recommended to group events through multiple event enumerations, and group monitoring methods through the **ListenGroupAttribute**.
* When the first parameter of the method that receives the event is named **eventType**, the event type is automatically sent, and the other parameters are shifted backward in turn. It can be used to handle situations where multiple events trigger the same method.
* **enum** / **string** type events use the **UEvent** interface, **class** / **struct** type events use the **UEvent\<T\>** interface, the interface does not do Complete type constraints, but be sure to call in accordance with the convention to avoid unexpected problems.
* Although multiple types of events are supported at the same time, it is still strongly recommended to use only one type of event format in a project.

### 1.6 Start
Copy `Events` folder into `UnityProject/Assets/Plugins/`.

***

## 2 Component

### 2.1 Event Manager
Used to manage all event dispatchers.

### 2.2 Event Dispatcher
Used to manage listener registration/de-registeration and event dispatcher for a specific event type.

### 2.3 Event Listener
Used to manage the event listener registration/de-registration of a specified object.

### 2.4 Object  Listener
The user class implements the event mechanism by inheriting the class or initializing the class by itself, which can automatically register and de-register the object, and provide quick-call listener registration, removal, and event distribution interfaces.

### 2.5 Unity MonoListener
Same as ObjectListener's interface，uesd for MonoBehaviour GameObject.

### 2.6 Event Handler
#### 2.6.1 EventHandler
|Property|Description|Remarks|
|-|-|-|
|Type|Event definition object type|Event definition `enum/string/class/struct` and other objects Type, equivalent to `typeof(X)`|
|EventType|The value of the listener event|1. For `enum/string` type events, it represents the specific enum value; 2. For `class/struct` events, it also appears as `Type` in the grouping definition of the listener, but it is actually An instance of this type is represented when the event is sent, and the content of the event can be included. |
|Group|Monitor group||
|Target|Event target object||
|Priority|Priority||
|Interrupt|Whether to interrupt the event queue||
|Method|Listen method|For the non-parameter delegated listening, the delegate will be automatically converted to MehtodInfo for execution|
|Parameters|Parameters of the listen method||
|Action|Listen delegate Action||
#### 2.6.2 EventHandler\<T\>
|Property|Description|Remarks|
|-|-|-|
|ActionT|Listen delegate Action\<T\>||
|ActionArgs|Listen delegate Action\<obj[]\>||
|ActionTArgs|Listen delegate Action\<T, object[]\>||

***

## 3 Optional Attribute

### 3.1 EventEnumAttribute
This attribute is a reserved function, and it may be used to automatically acquire event definitions scattered in different assemblies by reflection in the later stage. Currently, it has no function.
### 3.2 ListenAttribute
It is used to mark the method that needs to listen the specified event in class.
The event listener will automatically search for all methods containing this attribute and register for listen.
* **Property**
   * **Type** : Event type
   * **Priority** : Priority
   * **Interrupt** : Interrupt event queue
* **Attention**
   * Priority does not limit the value range, and controls itself according to the actual needs of the project. After setting a non-zero value, the event will be triggered from high to low.
   * You can mark to listen to multiple events.
   * Must be an instance object.
   * Must be a non-static method.

### 3.3 ListenTypeAttribute
The method with this attribute can receive all events of the specified event type, traverse all enumerations and register all listeners.
* **Property**
   * **Type** : Event enum type
   * **Priority** : Priority
   * **Interrupt** : Interrupt event queue

### 3.4 ListenGroupAttribute
The event will only be triggered when the event sent through the `DispatchGroup` interface corresponds to the group in the attribute tag.
* **Property**
   * **Type** : Event type
   * **Group** : Event group type
   * **Priority** : Priority
   * **Interrup**t : Interrupt event queue
*  **Attention**
   * Monitors added using other attribute tags are not grouped by default.
   * If the group is empty, it is equivalent to `ListenAttribute`, and it can accept listeners from any same event without a group.

***

## 4 API

### 4.1 Event Definition
``` cs
/* 
UEvent recommends using enumeration types to define events, and implement event grouping functions through different enumerations.
If you need to use string / class / struct types to define events, see the `Compatibility Features` section for details.
*/

// EventEnumAttributte is a reserved label and has no function temporarily.
[EventEnum]
public enum GameEventType
{
	GameStart,
	GameFinish,
}
```

### 4.2 Auto Listener
``` cs
// Any C# class gains monitoring capabilities.
public class TestClass : EventListener
{
}

// Unity MonoBehaviour object with listen capabilities.
public class UnityTestClass : MonoEventListener
{
}
```

### 4.3 Manual Listener
``` cs
// Generate a listener for the specified object, executed when the general object is generated.
var listener = new EventListener(this);

// Register all the listening methods of the listener, generally executed when the object takes effect.
EventListener.Register();

// Unregister all listening methods, generally executed when the object fails.
EventListener.DeRegister();
```

### 4.4 Listen Event
``` cs
// Listen to a single event, no priority.
[Listen(GameEvent.GameStart)]
public void TestMethod()
{
}

// Listen to a single event, set priority and interrupt.
[Listen(GameEvent.GameStart, 2, false)]
public void TestMethod()
{
}

// Monitor multiple events, no priority.
[Listen(GameEvent.GameStart, GameEvent.GameEnd)]
public void TestMethod()
{
}
```

### 4.5 Listen Event Type
``` cs
[ListenType(typeof(GameEvent))]
public void TestMethod()
{
}
```

### 4.6 Listen Group
``` cs
[ListenGroup(GameEvent.GameStart, "Player")]
public void TestMethod()
{
}
```

### 4.7 Maunal Register / Deregister Event
``` cs
// Add listener
UEvent.Listen<T>(eventType, this, methodInfo, group, priorty, interrupt);
UEvent.Listen<T>(eventType, action, group, priorty, interrupt);

// Whether to include listener
UEvent.Contains<T>()(eventType);

// Get listener with event type and target.
UEvent.Get(eventType);
UEvent.Get(eventType, target);

// Remove listener
UEvent.Remove(eventType, this, method);
UEvent.Remove(eventType, action);
```

### 4.8 Dispatch Event
``` cs
UEvent.Dispatch(eventType, args);
```

### 4.9 Dispatch Event (Thread Safety for Unity)
``` cs
// This interface will delegate events to the Unity main thread for execution, only available in Unity extensions.
UEvent.DispatchSafe(eventType, args);
```

### 4.10 Dispatch to Target
``` cs
UEvent.DispatchTo(eventType, target, args);
```

### 4.11 Dispatch to Group
``` cs
// If the `Group` parameter is empty, it is equivalent to calling the `Dispatch` interface, and the event will be sent to any listener method without a group.
UEvent.DispatchGroup(eventType, group, args);
```

### 4.12 Full API
If you want to get the complete function of **UEvent** or call the internal interface, you can call it by using the following methods. The interface is slightly different from the quick call interface mentioned above. You can get **EventDispatcher** first through **EventManager**, then call the specific internal interface, for example:
``` cs
EventManager.GetDispatcher<T>().AddListener<T>(eventType, action, group, priorty, interrupt);

EventManager.GetDispatcher<T>().RemoveListener<T>(eventType, action);
```

***

## 5 Compatibility Features

### 5.1 Performance Optimization
#### 5.1.1 Preloading
When a type is activated for the first time and registers for listening events, it is necessary to obtain all the methods of the type that need to be monitored and the attributes of the monitoring through reflection. The performance overhead due to reflection cannot be avoided, but it can be preloaded and cached. To reduce the overhead of instant use, so when necessary, you can actively call the following method at an appropriate time such as program startup or loading, and pre-register and cache the specified type of monitoring event information.
``` cs
public void PreRegister()
{
	var type = typeof(SomeClass);
	EventListener.Register(type);
}
```

### 5.2 Compatibility List
In order to be compatible with the existing event type definition methods in various projects, partial support for string / class / struct type events is provided, which can realize basic functions such as automatic/manual binding and event sending, but some functions will also be lost , See the function support list for details:

|Function|Support|
|---|---|
|Auto / Manual listen|√|
|Send event|√|
|Listen priority|√|
|Interrupt event queue|√|
|Event group type|×|
|Listen group **ListenGroupAttribute**|√|
|Listen event group type **ListenTypeAttribute**|×|

### 5.3 >string Event
``` cs
// Define event type
public static class StringEventDefine
{
	public const string Event01 = "Event01";
	public const string Event02 = "Event02";
}

// Register listener
UEvent.Listen(StringEventDefine.Event01, Receive);

// Dispatch event
UEvent.Dispatch(StringEventDefine.Event01, "Message");

// Remove listener
UEvent.Remove(StringEventDefine.Event01, Receive);

// Receive method
public void Receive(string message)
{
    Console.WriteLine(message);
}
```

### 5.4 class / struct Event
``` cs
// Define event type
public class ClassEventDefine
{
	public string Message;
}

// Register listener
UEvent.Listen<ClassEventDefine>(Receive);

// Dispatch event
UEvent.Dispatch(new ClassEventDefine()
{
	Message = "Message"
});

// Remove listener
UEvent.Remove<ClassEventDefine>(Receive);

// Receive method
public void Receive(ClassEventDefine evt)
{
	Console.WriteLine(evt.Message);
}
```
