using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public struct MyFormattable : IFormattable, IEquatable<MyFormattable>
{
    public int value;

    public MyFormattable(int value)
    {
        this.value = value;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        Debug.Log($"IFormattable ToString: {format}");
        return value.ToString(format, formatProvider);
    }

    public static bool operator ==(MyFormattable left, MyFormattable right)
    {
        return true;
    }

    public static bool operator !=(MyFormattable left, MyFormattable right)
    {
        return true;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool Equals(MyFormattable other)
    {
        throw new NotImplementedException();
    }
}

public class Scripting2 : MonoBehaviour
{
    private LinkedList<int> l = new LinkedList<int>();
    private int val;
    private Action action;
    private int[] arr;
    private long before = 0;
    private int frame = 0;
    private string s;

    private void Awake()
    {
        // Type2 a = new Type2(5);

        // Type1 b = a;
        // Debug.Log($"b={b.value}");

        // l.AddLast(0);
        // l.AddLast(1);
        // l.AddLast(2);

        // Debug.Log($"{new Dictionary<int, int>().GetEnumerator().GetType().IsValueType}");

        // DoSomething(new MyDerived());

        // MyFormattable myFormattable = new MyFormattable(3);
        // string s1 = string.Format("a{0}a", myFormattable);
        // // string s2 = $"a{myFormattable:5}a";
        // Debug.Log(s1);

        // MyAsync();
    }

    private void Update()
    {
        // if (frame == 1000)
        // {
        //     before = GC.GetTotalMemory(false);

        //     for (int i = 0; i < 10000; ++i)
        //     {
        //         int a = 3;
        //         // s = string.Format("<{0}>", a); // 659456
        //         // s = $"<{a}>"; // 671744
        //         s = $"<{a.ToString()}>"; // 659456
        //         // s = string.Format("<{0}>", a.ToString()); // 655360

        //         // s = a.ToString(); // 331776
        //     }
        // }

        // if (frame == 1001)
        // {
        //     long now = GC.GetTotalMemory(false);
        //     Debug.Log($"Memory: {now - before}");
        // }
        // for (int i = 0; i < 100000; ++i)
        // {
        //     // IEnumerator<int> enumerator = l.GetEnumerator();
        //     // enumerator.MoveNext();
        //     // val = enumerator.Current;

        //     foreach (int v in l)
        //     {
        //         val = v;
        //     }
        // }

        // for (int i = 0; i < 100000; ++i)
        // {
        //     // int index = i;
        //     // action = DoSomething;
        //     val = i;
        // }

        for (int i = 0; i < 100; ++i)
        {
            MyAsync();
        }

        ++frame;
    }

    public class MyBase
    {
        public static bool operator ==(MyBase left, MyDerived right)
        {
            return true;
        }
        public static bool operator !=(MyBase left, MyDerived right)
        {
            return true;
        }
        
    }

    public class MyDerived : MyBase { }

    public void DoSomething(MyBase b)
    {
        Debug.Log("DoSomething Base");
    }

    public void DoSomething<T>(T t)
    {
        Debug.Log("DoSomething <T>");
    }

    private IEnumerator MyCoroutine()
    {
        yield return null;
    }

    private async UniTaskVoid MyAsync()
    {
        // Debug.Log("MyAsync 1");
        await Task.Yield();
        // Debug.Log("MyAsync 2");
        // return 3;

        // await UniTask.Yield();

        // Debug.Log($"ThreadID AsyncWork a = {Thread.CurrentThread.ManagedThreadId}");
        // int value = await new MyTask<int>(() =>
        // {
        //     Debug.Log($"ThreadID Task = {Thread.CurrentThread.ManagedThreadId}");
        //     return 5;
        // });
        // Debug.Log($"value={value}, ThreadID AsyncWork b = {Thread.CurrentThread.ManagedThreadId}");
        // return value;

        // await Awaitable.NextFrameAsync();
    }

    // public void Generic<T>(T obj)
    // {
    //     Debug.Log($"Generic<T>: {obj.GetType()}");
    // }

    // public void Generic<int>(int obj)
    // {
    //     Debug.Log($"Generic<int>: {obj.GetType()}");
    // }

    public class Type1
    {
        public int value;
    }

    public class Type2
    {
        public Type1 data;

        public static implicit operator Type1(Type2 t) => t.data;

        public Type2(int value)
        {
            data = new Type1 { value = value };
        }
    }

    public (int, int) GetValueTuple()
    {
        var a = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
        return (a.Item9, a.Item1);
    }
}

public struct MyAwaiter<T> : INotifyCompletion
{
    private MyTask<T> task;

    public bool IsCompleted => task.IsTaskCompleted;

    public T GetResult()
    {
        return task.Result;
    }

    public MyAwaiter(MyTask<T> task)
    {
        this.task = task;
    }

    public void OnCompleted(Action continuation)
    {
        task.OnTaskCompleted += continuation;
        task.Start();
    }
}

public class MyTask<T>
{
    private Thread thread;
    public T Result { get; private set; }
    public bool IsTaskCompleted { get; private set; } = false;
    public event Action OnTaskCompleted;

    public MyTask(Func<T> func)
    {
        thread = new Thread(() =>
        {
            Result = func();
            IsTaskCompleted = true;
            OnTaskCompleted?.Invoke();
        });
    }

    public MyAwaiter<T> GetAwaiter()
    {
        return new MyAwaiter<T>(this);
    }

    public void Start()
    {
        thread.Start();
    }
}
