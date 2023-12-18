namespace LitEcs;

public interface ISystem
{
    void Run();
}

public interface ISystem<C1> where C1: struct
{
    void Run(int count, C1[] s1);
}

public interface ISystem<C1, C2> where C1: struct where C2: struct
{
    void Run(int count, C1[] s1, C2[] s2);
}

public interface ISystem<C1, C2, C3>  where C1: struct where C2: struct where C3: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3);
}

public interface ISystem<C1, C2, C3, C4> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4);
}

public interface ISystem<C1, C2, C3, C4, C5> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct where C5: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4, C5[] s5);
}


public interface ISystem<C1, C2, C3, C4, C5, C6> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct where C5: struct where C6: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4, C5[] s5, C6[] s6);
}

public interface ISystem<C1, C2, C3, C4, C5, C6, C7> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct where C5: struct where C6: struct
    where C7: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4, C5[] s5, C6[] s6, C7[] s7);
}

public interface ISystem<C1, C2, C3, C4, C5, C6, C7, C8> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct where C5: struct where C6: struct
    where C7: struct where C8: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4, C5[] s5, C6[] s6, C7[] s7, C8[] s8);
}

public interface ISystem<C1, C2, C3, C4, C5, C6, C7, C8, C9> 
    where C1: struct where C2: struct where C3: struct
    where C4: struct where C5: struct where C6: struct
    where C7: struct where C8: struct where C9: struct
{
    void Run(int count, C1[] s1, C2[] s2, C3[] s3, C4[] s4, C5[] s5, C6[] s6, C7[] s7, C8[] s8, C9[] s9);
}