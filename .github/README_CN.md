# UEvent 

**UEvent** 是一个可以使用在Unity和原生.Net环境下的通用消息事件组件，通过事件机制可以提供强大的解耦能力。

![license](https://img.shields.io/github/license/ls9512/UEvent)
[![openupm](https://img.shields.io/npm/v/com.ls9512.uevent?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.ls9512.uevent/)
![topLanguage](https://img.shields.io/github/languages/top/ls9512/UEvent)
![size](https://img.shields.io/github/languages/code-size/ls9512/UEvent)
![issue](https://img.shields.io/github/issues/ls9512/UEvent)
![last](https://img.shields.io/github/last-commit/ls9512/UEvent)
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)

[[English Documents Available]](README.md)

> 官方交流QQ群：[1070645638](https://jq.qq.com/?_wv=1027&k=ezkLnUln)

<!-- TOC -->

- [UEvent](#uevent)
	- [1 简介](#1-简介)
		- [1.1 环境](#11-环境)
		- [1.2 文件夹](#12-文件夹)
		- [1.3 特性](#13-特性)
		- [1.4 结构](#14-结构)
		- [1.5 约定](#15-约定)
		- [1.6 接入](#16-接入)
	- [2 组件](#2-组件)
		- [2.1 事件管理器 EventManager](#21-事件管理器-eventmanager)
		- [2.2 事件分发器 EventDispatcher](#22-事件分发器-eventdispatcher)
		- [2.3 事件监听器 EventListener](#23-事件监听器-eventlistener)
		- [2.4 Object 事件监听器 ObjectListener](#24-object-事件监听器-objectlistener)
		- [2.5 Unity MonoBehaviour 事件监听器 MonoListener](#25-unity-monobehaviour-事件监听器-monolistener)
		- [2.6 监听事件处理器 EventHandler](#26-监听事件处理器-eventhandler)
			- [2.6.1 EventHandler](#261-eventhandler)
			- [2.6.2 EventHandler\<T\>](#262-eventhandlert)
	- [3 可选特性标签 Attribute](#3-可选特性标签-attribute)
		- [3.1 事件定义枚举 EventEnumAttribute](#31-事件定义枚举-eventenumattribute)
		- [3.2 事件监听 ListenAttribute](#32-事件监听-listenattribute)
		- [3.3 类型监听 ListenTypeAttribute](#33-类型监听-listentypeattribute)
		- [3.4 监听分组 ListenGroupAttribute](#34-监听分组-listengroupattribute)
	- [4 API](#4-api)
		- [4.1 事件定义](#41-事件定义)
		- [4.2 自动监听器](#42-自动监听器)
		- [4.3 手动监听器](#43-手动监听器)
		- [4.4 监听事件](#44-监听事件)
		- [4.5 监听类型](#45-监听类型)
		- [4.6 监听分组](#46-监听分组)
		- [4.7 手动注册/注销事件](#47-手动注册注销事件)
		- [4.8 发送事件](#48-发送事件)
		- [4.9 发送事件 (Unity线程安全)](#49-发送事件-unity线程安全)
		- [4.10 发送事件到对象](#410-发送事件到对象)
		- [4.11 发送事件到分组](#411-发送事件到分组)
		- [4.12 Full API](#412-full-api)
	- [5 兼容性功能](#5-兼容性功能)
		- [5.1 性能优化](#51-性能优化)
			- [5.1.1 预加载](#511-预加载)
		- [5.2 兼容性列表](#52-兼容性列表)
		- [5.3  类型事件](#53--类型事件)
		- [5.4 class / struct 类型事件](#54-class--struct-类型事件)

<!-- /TOC -->

## 1 简介
### 1.1 环境
![Unity: 2019.4.3f1](https://img.shields.io/badge/Unity-2017+-blue) 
![.NET 4.x](https://img.shields.io/badge/.NET-4.x-blue) 

### 1.2 文件夹
* **Samples**: 例程文件夹，实际项目中可删除以减小空间占用。
* **CSharp**: 完全 .Net 实现的核心功能部分，可在 .Net 环境下独立使用。
* **Unity**: 依赖 UnityEngine 等 Unity 类库实现的额外功能，在 Unity 环境中工作时需要配合 Core 文件夹中的代码一起使用。

### 1.3 特性
* 支持通过枚举定义多组事件，按单一事件或者按事件类型进行监听。
* 同时支持 enum / string / class / struct 类型的事件定义最小化改动兼容不同项目。
* 支持监听接收事件优先级。
* 支持事件分组发送或者针对特定目标对象发送。
* 支持特定监听器中断整个监听事件队列。
* 同时支持普通方法和委托形式的事件。
* 提供 `ObjectListener` 和 `MonoListener` 基类，使任何类获得自动注册/移除监听的特性，也可以自行实现 `IEventListener` 接口。

### 1.4 结构
* 事件管理器 **EventManager** -> 事件分发器 **EventDispatcher**-> 事件处理器 **EventHandler**
* 事件监听器内部实现 **EventListener** -> 事件分发器 **EventDispatcher**
* 事件监听器外部实现 **UserEventListener**

### 1.5 约定
* 每一种类型的事件，使用一种枚举类型比如: AppEvent, GameEvent 等等，每种事件类型会对应一个 事件分发器 实例。
* 可以每个项目只使用一种事件类型，但不推荐。
* 方法型监听需要指定绑定对象，而委托型监听不需要指定对象。
* 委托类型的监听方法，如果有参数，则参数使用通用 **object[]** 形式，因而可能需要指定 **eventType** 来进行处理。
* 推荐通过多个事件枚举来对事件进行分组，通过监听分组特性来对监听方法进行分组。
* 当接受事件的方法第一个参数名为 **eventType** 时，会自动发送事件类型，其他参数依次后移。可用于处理需要区分多个事件触发同一方法的情况。
* **enum** / **string** 类型事件使用 **UEvent** 接口，**class** / **struct** 类型事件使用 **UEvent\<T\>** 接口，接口没有做完整的类型约束，但请务必按照约定调用以避免不可预期的问题。
* 尽管同时支持多种类型的事件，但任然强烈建议在一个项目中只使用一种类型的事件格式。

### 1.6 接入
将 `Events` 文件夹整个放入 `UnityProject/Assets/Plugins/` 目录下即可。

***

## 2 组件

### 2.1 事件管理器 EventManager
用于管理所有的事件分发器。

### 2.2 事件分发器 EventDispatcher
用于管理指定一种事件类型的监听注册/移除和事件分发。

### 2.3 事件监听器 EventListener
用于管理指定一个对象的事件监听注册/反注册。

### 2.4 Object 事件监听器 ObjectListener
普通类型通过继承该类或者使用自身初始化该类来实现事件机制，可以对该对象自动进行注册、反注册，同时提供快捷调用的监听注册、移除、事件分发接口。

### 2.5 Unity MonoBehaviour 事件监听器 MonoListener
与 ObjectListener 接口相同，提供给 MonoBehaviour 游戏对象使用。

### 2.6 监听事件处理器 EventHandler
#### 2.6.1 EventHandler
|属性|描述|备注|
|-|-|-|
|Type|事件定义类型|事件定义 `enum/string/class/struct` 等对象的 Type，相当于 `typeof(X)`|
|EventType|监听事件的值|1.对于 `enum/string` 类型事件，则表示具体的枚举值；2.对于 `class/struct` 事件，在监听器的分组定义中同样表现为 `Type`，但实际发送事件时表现该类型的一个实例，可以包含事件内容。|
|Group|监听分组||
|Target|事件目标对象||
|Priority|优先级||
|Interrupt|是否中断事件队列||
|Method|监听方法|对于无参数委托形式的监听，会自动将委托转换为 MehtodInfo 执行|
|Parameters|监听方法的参数||
|Action|监听委托 Action||

#### 2.6.2 EventHandler\<T\>
|属性|描述|备注|
|-|-|-|
|ActionT|监听委托 Action\<T\>||
|ActionArgs|监听委托 Action\<obj[]\>||
|ActionTArgs|监听委托 Action\<T, object[]\>||

***

## 3 可选特性标签 Attribute

### 3.1 事件定义枚举 EventEnumAttribute
该特性标签为预留功能，后期可能用于反射自动获取分散于不同程序集中的事件定义，目前暂无功能。
### 3.2 事件监听 ListenAttribute
用于标记该类型内需要监听指定事件的方法。
事件监听器会自动搜索所有包含该特性的方法，进行监听注册。
* **属性**
   * **Type** : 事件类型
   * **Priority** : 优先级
   * **Interrupt** : 是否中断事件队列
* **注意事项**
   * 优先级不限制取值范围，根据项目实际需要自行控制，设置非0的值后，将由高到低触发事件
   * 可以标记监听多个事件
   * 必须是实例对象
   * 必须是非静态方法

### 3.3 类型监听 ListenTypeAttribute
加上该标记的方法能够接收指定事件类型的所有事件，会遍历所有枚举并注册所有监听
* **属性**
   * **Type** : 事件枚举类型
   * **Priority** : 优先级
   * **Interrupt** : 是否中断事件队列

### 3.4 监听分组 ListenGroupAttribute
只有通过 `DispatchGroup` 接口发送的事件，与该特性标签中的分组对应时才会触发事件。
* **属性**
   * **Type** : 事件类型
   * **Group** : 事件分组
   * **Priority** : 优先级
   * **Interrup**t : 是否中断事件队列
*  **注意事项**
   * 使用其他特性标签添加的监听默认是没有分组的。
   * 分组如果为空则等效于 `ListenAttribute`，可以接受来自任何无分组的同一事件的监听。

***

## 4 API

### 4.1 事件定义
``` cs
/* 
UEvent 推荐使用枚举类型定义事件，通过不同的枚举实现事件分组功能。
如果需要使用 string / class / struct 类型定义事件，详见 `兼容性功能` 部分
*/

// EventEnumAttributte 为预留标签，暂无功能
[EventEnum]
public enum GameEventType
{
	GameStart,
	GameFinish,
}
```

### 4.2 自动监听器
``` cs
// 任意 C# 类获得监听能力
public class TestClass : EventListener
{
}

// 拥有监听能力的 Unity MonoBehaviour 对象
public class UnityTestClass : MonoEventListener
{
}
```

### 4.3 手动监听器
``` cs
// 生成指定对象的监听器，一般对象生成时执行
var listener = new EventListener(this);

// 注册该监听器的所有监听方法，一般在对象生效时执行
EventListener.Register();

// 注销所有监听方法，一般在对象失效时执行
EventListener.DeRegister();
```

### 4.4 监听事件
``` cs
// 监听单个事件，无优先级
[Listen(GameEvent.GameStart)]
public void TestMethod()
{
}

// 监听单个事件，设置优先级和中断
[Listen(GameEvent.GameStart, 2, false)]
public void TestMethod()
{
}

// 监听多个事件，无优先级
[Listen(GameEvent.GameStart, GameEvent.GameEnd)]
public void TestMethod()
{
}
```

### 4.5 监听类型
``` cs
[ListenType(typeof(GameEvent))]
public void TestMethod()
{
}
```

### 4.6 监听分组
``` cs
[ListenGroup(GameEvent.GameStart, "Player")]
public void TestMethod()
{
}
```

### 4.7 手动注册/注销事件
``` cs
// 添加监听
UEvent.Listen<T>(eventType, this, methodInfo, group, priorty, interrupt);
UEvent.Listen<T>(eventType, action, group, priorty, interrupt);

// 是否包含监听
UEvent.Contains<T>()(eventType);

// 获取监听
UEvent.Get(eventType);
UEvent.Get(eventType, target);

// 移除监听
UEvent.Remove(eventType, this, method);
UEvent.Remove(eventType, action);
```

### 4.8 发送事件
``` cs
UEvent.Dispatch(eventType, args);
```

### 4.9 发送事件 (Unity线程安全)
``` cs
// 该接口会将事件以委托形式转交给Unity主线程执行，仅Unity扩展中可用。
UEvent.DispatchSafe(eventType, args);
```

### 4.10 发送事件到对象
``` cs
UEvent.DispatchTo(eventType, target, args);
```

### 4.11 发送事件到分组
``` cs
// `Group` 参数如果为空，则等效为调用 `Dispatch` 接口，事件将发送给任何没有分组的监听方法。
UEvent.DispatchGroup(eventType, group, args);
```

### 4.12 Full API
如果想获得 **UEvent** 的完整功能或调用内部接口，则可以通过使用以下方式访问调用，接口与上文提到的快速调用接口略有差异，通过 **EventManager** 先获取到对应事件的 **EventDispatcher**，再调用具体内部接口，例如：
``` cs
EventManager.GetDispatcher<T>().AddListener<T>(eventType, action, group, priorty, interrupt);

EventManager.GetDispatcher<T>().RemoveListener<T>(eventType, action);
```

***

## 5 兼容性功能
### 5.1 性能优化
#### 5.1.1 预加载
当某个类型被第一次激活并注册监听事件时，需要通过反射获取该类型所有需要被监听的方法以及监听的规则特性，由于反射的性能开销无法被避免，但可以通过预先加载和缓存机制来减少即时使用时的开销，因此必要的时候，可以在程序启动或者加载等合适的时机，主动调用以下方法，预先注册缓存指定类型的监听事件信息。
``` cs
public void PreRegister()
{
	var type = typeof(SomeClass);
	EventListener.Register(type);
}
```

### 5.2 兼容性列表
为了兼容各种项目中已有的事件类型定义方式，提供了对 string / class / struct 类型事件的部分支持，可以实现事件的自动/手动绑定和事件发送等基本功能，但也会失去部分功能，详见功能支持列表：

|功能|支持|
|---|---|
|自动/手动监听|√|
|发送事件|√|
|监听优先级|√|
|监听队列中断|√|
|定义分组分类|×|
|监听分组 **ListenGroupAttribute**|√|
|监听同类型事件 **ListenTypeAttribute**|×|

### 5.3  类型事件
``` cs
// 定义
public static class StringEventDefine
{
	public const string Event01 = "Event01";
	public const string Event02 = "Event02";
}

// 监听
UEvent.Listen(StringEventDefine.Event01, Receive);

// 发送
UEvent.Dispatch(StringEventDefine.Event01, "Message");

// 移除
UEvent.Remove(StringEventDefine.Event01, Receive);

// 接收
public void Receive(string message)
{
    Console.WriteLine(message);
}
```

### 5.4 class / struct 类型事件
``` cs
// 定义
public class ClassEventDefine
{
	public string Message;
}

// 监听
UEvent.Listen<ClassEventDefine>(Receive);

// 发送
UEvent.Dispatch(new ClassEventDefine()
{
	Message = "Message"
});

// 移除
UEvent.Remove<ClassEventDefine>(Receive);

// 接收
public void Receive(ClassEventDefine evt)
{
	Console.WriteLine(evt.Message);
}
```
