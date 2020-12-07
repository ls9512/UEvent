# UEvent 

**UEvent** 是一个可以使用在Unity和原生.Net环境下的通用消息事件组件，通过事件机制可以提供强大的解耦能力。

![topLanguage](https://img.shields.io/github/languages/top/ls9512/UEvent)
![size](https://img.shields.io/github/languages/code-size/ls9512/UEvent)
![issue](https://img.shields.io/github/issues/ls9512/UEvent)
![license](https://img.shields.io/github/license/ls9512/UEvent)
![last](https://img.shields.io/github/last-commit/ls9512/UEvent)
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)

[[English Documents Available]](README.md)

<!-- vscode-markdown-toc -->
* 1. [简介](#)
	* 1.1. [环境](#-1)
	* 1.2. [文件](#-1)
	* 1.3. [特性](#-1)
	* 1.4. [结构](#-1)
	* 1.5. [约定](#-1)
	* 1.6. [接入](#-1)
* 2. [组件](#-1)
	* 2.1. [事件管理器 EventManager](#EventManager)
	* 2.2. [事件分发器 EventDispatcher](#EventDispatcher)
	* 2.3. [事件监听器 EventListener](#EventListener)
	* 2.4. [Object 事件监听器 ObjectListener](#ObjectObjectListener)
	* 2.5. [Unity MonoBehaviour 事件监听器 MonoListener](#UnityMonoBehaviourMonoListener)
	* 2.6. [监听事件处理器 EventHander](#EventHander)
* 3. [可选特性标签 Attribute](#Attribute)
	* 3.1. [事件定义枚举 EventEnumAttribute](#EventEnumAttribute)
	* 3.2. [事件监听 ListenAttribute](#ListenAttribute)
	* 3.3. [类型监听 ListenTypeAttribute](#ListenTypeAttribute)
	* 3.4. [监听分组 ListenGroupAttribute](#ListenGroupAttribute)
* 4. [API](#API)
	* 4.1. [事件定义](#-1)
	* 4.2. [自动监听器](#-1)
	* 4.3. [手动监听器](#-1)
	* 4.4. [监听事件](#-1)
	* 4.5. [监听类型](#-1)
	* 4.6. [监听分组](#-1)
	* 4.7. [手动注册/注销事件](#-1)
	* 4.8. [发送事件](#-1)
	* 4.9. [发送事件 (Unity线程安全)](#Unity)
	* 4.10. [发送事件到对象](#-1)
	* 4.11. [发送事件到分组](#-1)
	* 4.12. [Full API](#FullAPI)
* 5. [兼容性功能](#-1)
	* 5.1. [兼容性列表](#-1)
	* 5.2. [string 类型事件](#string)
	* 5.3. [class / struct 类型事件](#classstruct)

<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->
##  1. <a name=''></a>简介
###  1.1. <a name='-1'></a>环境
* Unity : 2017+
* .Net : 4.x+

###  1.2. <a name='-1'></a>文件夹
* **Example**: 例程文件夹，实际项目中可删除以减小空间占用。
* **Script / CSharp**: 完全 .Net 实现的核心功能部分，可在 .Net 环境下独立使用。
* **Script / Unity**: 依赖 UnityEngine 等 Unity 类库实现的额外功能，在 Unity 环境中工作时需要配合 Core 文件夹中的代码一起使用。
* **Script / * / Ext**: 用来兼容 string / class / struct 类型事件的临时实现代码，以后可能被替换。

###  1.3. <a name='-1'></a>特性
* 支持通过枚举定义多组事件，按单一事件或者按事件类型进行监听。
* 同时支持 enum / string / class / struct 类型的事件定义最小化改动兼容不同项目。
* 支持监听接收事件优先级。
* 支持事件分组发送或者针对特定目标对象发送。
* 支持特定监听器中断整个监听事件队列。
* 同时支持普通方法和委托形式的事件。
* 提供 `ObjectListener` 和 `MonoListener` 基类，使任何类获得自动注册/移除监听的特性，也可以自行实现 `IEventListener` 接口。

###  1.4. <a name='-1'></a>结构
* 事件管理器 **EventManager** -> 事件分发器 **EventDispatcher**-> 事件处理器 **EventHandler**
* 事件监听器内部实现 **EventListener** -> 事件分发器 **EventDispatcher**
* 事件监听器外部实现 **UserEventListener**

###  1.5. <a name='-1'></a>约定
* 每一种类型的事件，使用一种枚举类型比如: AppEvent, GameEvent 等等，每种事件类型会对应一个 事件分发器 实例。
* 可以每个项目只使用一种事件类型，但不推荐。
* 方法型监听需要指定绑定对象，而委托型监听不需要指定对象。
* 委托类型的监听方法，参数使用通用的 **object[]** 形式，因而需要指定 **eventType** 来进行处理，所以委托强制约定格式为 **Action<T, object[]>**。
* 推荐通过多个事件枚举来对事件进行分组，通过监听分组特性来对监听方法进行分组。
* 当接受事件的方法第一个参数名为 **eventType** 时，会自动发送事件类型，其他参数依次后移。可用于处理需要区分多个事件触发同一方法的情况。
* **enum** / **string** 类型事件使用 **UEvent** 接口，**class** / **struct** 类型事件使用 **UEvent\<T\>** 接口，接口没有做完整的类型约束，但请务必按照约定调用以避免不可预期的问题。
* 尽管同时支持多种类型的事件，但任然强烈建议在一个项目中只使用一种类型的事件格式。

###  1.6. <a name='-1'></a>接入
将 `Events` 文件夹整个放入 `UnityProject/Assets/Plugins/` 目录下即可。

***

##  2. <a name='-1'></a>组件

###  2.1. <a name='EventManager'></a>事件管理器 EventManager
用于管理所有的事件分发器。

###  2.2. <a name='EventDispatcher'></a>事件分发器 EventDispatcher
用于管理指定一种事件类型的监听注册/移除和事件分发。

###  2.3. <a name='EventListener'></a>事件监听器 EventListener
用于管理指定一个对象的事件监听注册/反注册。

###  2.4. <a name='ObjectObjectListener'></a>Object 事件监听器 ObjectListener
普通类型通过继承该类或者使用自身初始化该类来实现事件机制，可以对该对象自动进行注册、反注册，同时提供快捷调用的监听注册、移除、事件分发接口。

###  2.5. <a name='UnityMonoBehaviourMonoListener'></a>Unity MonoBehaviour 事件监听器 MonoListener
与 ObjectListener 接口相同，提供给 MonoBehaviour 游戏对象使用。

###  2.6. <a name='EventHander'></a>监听事件处理器 EventHandler
* **Type** : 事件类型
* **Group** : 监听分组
* **Target** : 事件目标对象，可空
* **Priority** : 优先级
* **Interrupt** : 是否中断事件队列
* **Method** : 监听方法
* **Parameters** ：监听方法的参数
* **Action<T, object[]>** : 监听委托

***

##  3. <a name='Attribute'></a>可选特性标签 Attribute

###  3.1. <a name='EventEnumAttribute'></a>事件定义枚举 EventEnumAttribute
该特性标签为预留功能，后期可能用于反射自动获取分散于不同程序集中的事件定义，目前暂无功能。
###  3.2. <a name='ListenAttribute'></a>事件监听 ListenAttribute
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

###  3.3. <a name='ListenTypeAttribute'></a>类型监听 ListenTypeAttribute
加上该标记的方法能够接收指定事件类型的所有事件，会遍历所有枚举并注册所有监听
* **属性**
   * **Type** : 事件枚举类型
   * **Priority** : 优先级
   * **Interrupt** : 是否中断事件队列

###  3.4. <a name='ListenGroupAttribute'></a>监听分组 ListenGroupAttribute
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

##  4. <a name='API'></a>API

###  4.1. <a name='-1'></a>事件定义
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

###  4.2. <a name='-1'></a>自动监听器
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

###  4.3. <a name='-1'></a>手动监听器
``` cs
// 生成指定对象的监听器，一般对象生成时执行
var listener = new EventListener(this);

// 注册该监听器的所有监听方法，一般在对象生效时执行
EventListener.Register();

// 注销所有监听方法，一般在对象失效时执行
EventListener.DeRegister();
```

###  4.4. <a name='-1'></a>监听事件
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

###  4.5. <a name='-1'></a>监听类型
``` cs
[ListenType(typeof(GameEvent))]
public void TestMethod()
{
}
```

###  4.6. <a name='-1'></a>监听分组
``` cs
[ListenGroup(GameEvent.GameStart, "Player")]
public void TestMethod()
{
}
```

###  4.7. <a name='-1'></a>手动注册/注销事件
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

###  4.8. <a name='-1'></a>发送事件
``` cs
UEvent.Dispatch(eventType, args);
```

###  4.9. <a name='Unity'></a>发送事件 (Unity线程安全)
``` cs
// 该接口会将事件以委托形式转交给Unity主线程执行，仅Unity扩展中可用。
UEvent.DispatchSafe(eventType, args);
```

###  4.10. <a name='-1'></a>发送事件到对象
``` cs
UEvent.DispatchTo(eventType, target, args);
```

###  4.11. <a name='-1'></a>发送事件到分组
``` cs
// `Group` 参数如果为空，则等效为调用 `Dispatch` 接口，事件将发送给任何没有分组的监听方法。
UEvent.DispatchGroup(eventType, group, args);
```

###  4.12. <a name='FullAPI'></a>Full API
如果想获得 **UEvent** 的完整功能或调用内部接口，则可以通过使用以下方式访问调用，接口与上文提到的快速调用接口略有差异，通过 **EventManager** 先获取到对应事件的 **EventDispatcher**，再调用具体内部接口，例如：
``` cs
EventManager.GetDispatcher<T>().AddListener<T>(eventType, action, group, priorty, interrupt);

EventManager.GetDispatcher<T>().RemoveListener<T>(eventType, action);
```

***

##  5. <a name='-1'></a>兼容性功能
###  5.1. <a name='-1'></a>兼容性列表
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

###  5.2. <a name='string'></a>string 类型事件
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

###  5.3. <a name='classstruct'></a>class / struct 类型事件
``` cs
// 定义
public class ClassEventDefine
{
	public string Message;
}

// 监听
UEvent<ClassEventDefine>.Listen(Receive);

// 发送
UEvent<ClassEventDefine>.Dispatch(new ClassEventDefine()
{
	Message = "Message"
});

// 移除
UEvent<ClassEventDefine>.Remove(Receive);

// 接收
public void Receive(ClassEventDefine evt)
{
	Console.WriteLine(evt.Message);
}
```