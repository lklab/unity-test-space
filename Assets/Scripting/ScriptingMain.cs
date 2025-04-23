using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Jobs;

public class MyBase
{
    public readonly int val;

    public MyBase(int val)
    {
        this.val = val;
    }
}

public class MyDrived : MyBase
{
    public MyDrived(int val) : base(val)
    {

    }
}

class Animal {}
class Dog : Animal {}
class Cat : Animal {}

public class Meter
{
    public double Value { get; }

    public Meter(double value)
    {
        Value = value;
    }

    // double -> Meter 로 암시적 변환 정의
    public static implicit operator Meter(double value)
    {
        return new Meter(value);
    }
}

public interface IChapter2Interface
{
    void DoInterface();

    internal void Haha();

    public void DefaultMethod()
    {
        Debug.Log("DefaultMethod in IChapter2Interface");
    }

    public virtual void Broccoli()
    {
        Debug.Log("Broccoli");
    }

    public static int StaticValue = 0;

    public static void MyStatic()
    {

    }
}

public class Chapter2Base : IChapter2Interface
{
    public static int baseVal = 5;

    static Chapter2Base()
    {
        Debug.Log($"Chapter2Base static constructor baseVal={baseVal}");
        baseVal = 55;
        IChapter2Interface.MyStatic();
        IChapter2Interface.StaticValue = 10;
    }

    public Chapter2Base()
    {
        Debug.Log($"Chapter2Base");
    }

    ~Chapter2Base()
    {
        Debug.Log($"Chapter2Base Finalizer");
    }

    void IChapter2Interface.DoInterface() // 인터페이스로 캐스팅해야 호출가능
    {
        Debug.Log($"Chapter2Base DoInterface");
    }

    public virtual void DoA()
    {
        Debug.Log($"Chapter2Base DoA");
    }

    void IChapter2Interface.DefaultMethod()
    {
        throw new NotImplementedException();
    }

    public void DefaultMethod()
    {
        ((IChapter2Interface)this).DefaultMethod();
        Debug.Log("DefaultMethod in Chapter2Base");
    }

    void IChapter2Interface.Haha()
    {
        throw new NotImplementedException();
    }

    public void H2(IChapter2Interface i)
    {
        i.Haha();
    }
}

public class Chapter2Class : Chapter2Base
{
    public static int derivedVal = 2;

    static Chapter2Class()
    {
        // Debug.Log($"Chapter2Class static constructor baseVal={baseVal}");
        Debug.Log($"Chapter2Class static constructor 1");
        // derivedVal = baseVal;
        derivedVal = 22;
        Debug.Log($"Chapter2Class static constructor 2");
    }

    ~Chapter2Class()
    {
        Debug.Log($"Chapter2Class Finalizer");
    }

    public readonly Vector3 v = Vector3.one;

    public Chapter2Class(Vector3 v) : base()
    {
        Debug.Log($"Chapter2Class 1 v={this.v}");
        this.v = v;
        Debug.Log($"Chapter2Class 2 v={this.v}");
    }

    // public new void DoInterface()
    // {
    //     base.DoInterface();
    //     Debug.Log($"Chapter2Class DoInterface");
    // }

    public new void DoA()
    {
        Debug.Log($"Chapter2Class DoA");
    }

    public void SetGC()
    {
        GC.SuppressFinalize(this);
    }
}

public class Chapter2B
{
    protected Chapter2B()
    {
        VFunc();
    }

    protected virtual void VFunc()
    {
        Debug.Log("VFunc in B");
    }

    public virtual void MyGeneric<T>(T obj)
    {

    }
}

public class Chapter2BDerived : Chapter2B
{
    public readonly string msg = "Set by initializer";

    public Chapter2BDerived(string msg)
    {
        this.msg = msg;
    }

    protected override void VFunc()
    {
        Debug.Log(msg);
    }

    public override void MyGeneric<T>(T obj)
    {

    }
}

public struct SomeData
{
    public int value;

    public SomeData(int value)
    {
        this.value = value;
    }
}

[Serializable]
public struct SerializableData
{
    public int value;
}

public class SomeClass
{
    public int value;
}

public class ScriptingMain : MonoBehaviour
{
    [SerializeField] MyScriptableObject o = null;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        // Debug.Log("Awake 1");
        // IsAsTest();
        // Chapter2();
        // CoroutineTest();
        // VolatileTest();
        // METest1();
        // METest2();
        // METest3();
        // METest4();
        // METest5();
        // Debug.Log("Awake 2");
        text.text = "1";
    }

    // private void Update()
    // {
    //     NativeArray<SomeData> input = new NativeArray<SomeData>(1000000, Allocator.Persistent);
    //     NativeArray<SomeData> output = new NativeArray<SomeData>(1000000, Allocator.Persistent);
    //     for (int i = 0; i < input.Length; ++i)
    //     {
    //         input[i] = new SomeData(i);
    //     }

    //     var job = new MyJob()
    //     {
    //         input = input,
    //         output = output,
    //     };
    //     job.Schedule(input.Length, 64).Complete();

    //     input.Dispose();
    //     output.Dispose();
    // }

    private int METest5_Add(dynamic left, dynamic right)
    {
        Debug.Log($"{left}");
        return 5;
    }

    private void METest5_In(in SomeData data)
    {
        Debug.Log($"METest5_In: {data.value}");
    }

    private int METest5_field = 0;

    private bool METest5_Filter(Exception e)
    {
        LogAndText("Filter called.");
        METest5_field = 42; // 필터에서 값을 바꿈
        return false; // 필터 조건을 만족하지 않게 해서 catch 블록이 아닌 다음 catch로 넘어가게 함
    }

    private void METest5_ExceptionFilter()
    {
        try
        {
            throw new Exception("Boom!");
        }
        catch (Exception e) when (METest5_Filter(e))
        {
            LogAndText("Caught with filter. Field: " + METest5_field);
        }
        catch (Exception e)
        {
            LogAndText("Caught without filter. Field: " + METest5_field);
        }
    }

    private void METest5()
    {
        // Debug.Log($"Add: {METest5_Add(1, 2)}");
        // int x = 42;
        // dynamic d = x;

        // METest5_In(new SomeData(5));

        // METest5_ExceptionFilter();

        // Debug.Log(d.GetType()); // System.Int32

        Texture2D texture = new Texture2D(128, 128);
        LogAndText($"texture==null: {texture == null}");
        DestroyImmediate(texture);
        // Destroy(texture);
        LogAndText($"texture==null: {texture == null}");
    }

    private struct MyJob : IJobParallelFor
    {
        [Unity.Collections.ReadOnly]
        public NativeArray<SomeData> input;

        [WriteOnly]
        public NativeArray<SomeData> output;

        public void Execute(int index)
        {
            output[index] = new SomeData(input[index].value * 2);
        }
    }

    private void METest4()
    {
        Debug.Log("METest4 1");
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (sender, args) =>
        {
            Debug.Log($"in DoWork ThreadID={Thread.CurrentThread.ManagedThreadId}");
        };
        worker.RunWorkerCompleted += (sender, args) =>
        {
            Debug.Log($"in RunWorkerCompleted ThreadID={Thread.CurrentThread.ManagedThreadId}");
        };
        Debug.Log("METest4 2");
        worker.RunWorkerAsync();
        Debug.Log("METest4 3");
    }

    private async Task<int> METest3_task(int id, bool immediately)
    {
        if (immediately)
        {
            return 1;
        }

        Debug.Log($"in METest3_task {id} 1 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(1000 * id);
        Debug.Log($"in METest3_task {id} 2 ThreadID={Thread.CurrentThread.ManagedThreadId}");

        if (id == 5)
        {
            throw new Exception($"METest3_task {id} Exception");
        }

        return 1;
    }

    private async void METest3()
    {
        // Debug.Log($"call METest3_task 1 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        // var task = METest3_task(false);
        // Debug.Log($"call METest3_task 2 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        // await task.ConfigureAwait(false);
        // Debug.Log($"call METest3_task 3 ThreadID={Thread.CurrentThread.ManagedThreadId}");

        // Action asyncAction = async () => {
        //     Debug.Log($"asyncAction ThreadID={Thread.CurrentThread.ManagedThreadId}");
        //     await Task.Delay(1000);
        //     throw new Exception("in asyncAction");
        // };
        // asyncAction();

        // Action asyncAction2 = async () => {
        //     await Task.Delay(2000);
        //     Debug.Log("in asyncAction 2");
        // };
        // asyncAction2();

        // var task2 = Task.Run(async () => {
        //     Debug.Log($"task2 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        //     await Task.Delay(1000);
        //     Debug.Log($"task2 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        //     throw new Exception("in task");
        // });

        Debug.Log($"call METest3_task 1 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        var tasks = new List<Task>();
        for (int i = 0; i < 10; ++i)
        {
            tasks.Add(METest3_task(i, false));
        }
        Debug.Log($"call METest3_task 2 ThreadID={Thread.CurrentThread.ManagedThreadId}");
        await Task.WhenAll(tasks);
        Debug.Log($"call METest3_task 3 ThreadID={Thread.CurrentThread.ManagedThreadId}");
    }

    public class METest2Class
    {
        public event Action myAction;

        public static void MyStatic()
        {
            
        }
    }

    public class METest2Derived : METest2Class
    {
        public void Invoke()
        {
            // myAction?.Invoke();
        }

        public new static void MyStatic()
        {
            
        }
    }

    public class Fruit { }
    public class Apple : Fruit { }
    public class Mango : Fruit { }

    public class Animal2
    {
        public void Foo(Apple param) => Debug.Log("In Animal.Foo");
    }

    public class Tiger : Animal2
    {
        public void Foo(Fruit param) => Debug.Log("In Tiger.Foo");
    }

    private void METest2_Array(Fruit[] fruits)
    {
        fruits[0] = new Apple();
    }

    private async Task METest2_task()
    {
        Debug.Log("in METest2_task 1");
        await Task.Delay(5000);
        Debug.Log("in METest2_task 2");
    }

    private IEnumerator METest2_Coroutine()
    {
        Debug.Log("in METest2_Coroutine 1");
        yield return null;
        Debug.Log("in METest2_task 2");
    }

    private void METest2()
    {
        // SomeClass data = new SomeClass();
        // METest2_DoSomething(ref data);

        // METest2Class c = new METest2Class();
        // c.myAction += () => {};
        // c.myAction -= () => {};
        // c.myAction?.Invoke();

        var obj2 = new Tiger();
        obj2.Foo(new Apple());
        obj2.Foo(new Fruit());

        // Animal2 obj3 = new Tiger();
        // obj3.Foo(new Apple());

        // METest2Derived.MyStatic();

        // Fruit[] fruits = new Apple[1];
        // METest2_Array(fruits);

        // Debug.Log("call METest2_task 1");
        // var task = METest2_task();
        // Debug.Log("call METest2_task 2");

        // Debug.Log("call METest2_Coroutine 1");
        // var it = METest2_Coroutine();
        // Debug.Log("call METest2_Coroutine 2");
    }

    private void METest2_DoSomething(ref SomeClass data)
    {

    }

    public class METest1Class
    {
        public SomeData Value { get; set; }
        public SerializableData Broccoli { get; set; }
        public SomeData value2;

        public override bool Equals(object obj)
        {
            Debug.Log("METest1Class Equals");
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class METest1Derived : METest1Class
    {
        public override bool Equals(object obj)
        {
            Debug.Log("METest1Derived Equals");
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public struct METest1Struct
    {
        public int value;
        public string name;
    }

    private void METest1()
    {
        METest1Class c = new METest1Class();
        // METest1_UpdateData(c.Value);
        // c.Value.value = 55;

        var aPoint = new { X = 5, Y = 67 };
        Debug.Log($"aPoint X={aPoint.X} type={aPoint.GetType()}");
        var another = Transform(aPoint, (p) => new { X = 2, Y = 3 });

        static T2 Transform<T, T2>(T element, Func<T, T2> func)
        {
            return func(element);
        }
        Debug.Log($"another X={another.X} type={aPoint.GetType()}");

        var tuplePoint = (X: 5, Y: 67);
        Debug.Log($"tuplePoint X={tuplePoint.X} type={tuplePoint.GetType()}");
        (int Rise, int Run) tupleThree = tuplePoint;
        Debug.Log($"tupleThree Rise={tupleThree.Rise} type={tupleThree.GetType()}");
        var tupleFour = METest1_GetTuple();
        Debug.Log($"tupleFour a={tupleFour.a} type={tupleFour.GetType()}");

        Debug.Log($"null ref equals: {ReferenceEquals(null, null)}");

        METest1Derived derived = new METest1Derived();
        derived.Equals(5);

        METest1Struct st1 = new METest1Struct(){ value = 5, name = "55" };
        METest1Struct st2 = new METest1Struct(){ value = 5, name = "55" };
        Debug.Log($"st1 hash={st1.GetHashCode()}, st2 hash={st2.GetHashCode()}");
    }

    private (int a, int b) METest1_GetTuple() => (1, 2);

    private void METest1_UpdateData(ref SomeData data)
    {
        data.value = 55;
    }

    private volatile int counter = 0;

    private void VolatileTest()
    {
        Thread t1 = new Thread(IncrementCounter);
        Thread t2 = new Thread(DecrementCounter);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Debug.Log("Counter value: " + counter);
    }

    private void IncrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            counter++;
        }
    }

    private void DecrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            counter--;
        }
    }

    private void CoroutineTest()
    {
        StartCoroutine(MyCoroutine());
    }

    private IEnumerator MyCoroutine()
    {
        Debug.Log("in coroutine");
        yield return null;
    }

    private void Chapter2()
    {
        Debug.Log($"start Chapter2");
        Chapter2Class c = new Chapter2Class(Vector3.right);
        Debug.Log($"static derivedVal={Chapter2Class.derivedVal}");
        // c.SetGC();
        Debug.Log($"v={c.v}");

        IChapter2Interface i = c;
        i.DefaultMethod();
        i.Haha();

        // IChapter2Interface i = c;
        // c.DoInterface();
        // i.DoInterface();
        Chapter2Base b = c;
        b.DoA();
        // b.DoInterface();

        string s1 = "hi";
        s1 += " hello";
        s1 = string.Intern(s1);
        string s2 = "hi h";
        s2 += "ello";
        s2 = string.Intern(s2);
        Debug.Log($"{s1}=={s2}: {ReferenceEquals(s1, s2)}, {s1 == s2}");

        new Chapter2BDerived("Constructed in main");
    }

    private void IsAsTest()
    {
        int intVal = 5;
        object o = intVal;
        int? val1 = o as int?;
        if (val1 != null)
        {
            Debug.Log($"val1={val1.Value}");
        }
        else
        {
            Debug.Log($"val1 is null");
        }

        if (val1 is int inval1)
        {
            Debug.Log($"inval1={inval1}");
        }
        else
        {
            Debug.Log($"inval1 is null");
        }

        float? val2 = o as float?;
        if (val2 != null)
        {
            Debug.Log($"val2={val2.Value}");
        }
        else
        {
            Debug.Log($"val2 is null");
        }

        object o2 = new MyDrived(10);
        MyBase myBase = o2 as MyBase;
        if (myBase != null)
        {
            Debug.Log($"myBase={myBase.val}");
        }
        else
        {
            Debug.Log($"myBase is null");
        }

        Animal a = new Dog();
        o = a;

        Debug.Log(o is Animal);
        Debug.Log(o is Dog);
        Debug.Log(o is Cat);

        Debug.Log((o as Dog) != null);
        Debug.Log((o as Cat) != null);

        object o3 = 5.5;
        Meter meter = (Meter)o3;
        Debug.Log($"meter={meter.Value}");
    }

    public void LogAndText(string msg)
    {
        text.text += "\n" + msg;
        Debug.Log(msg);
    }
}
