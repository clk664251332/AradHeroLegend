using System;
//对于属性来说，基础值baseValue是没有通过装备加成的值
//对于战斗属性值来说，其大小被装备加成后的基本属性 以及 装备加成后的 自身属性影响
public class Value<T>
{
    public delegate T Filter(T lastValue, T newValue);

    private Action m_Set;
    /// <summary>
    /// 因数值改变而调用具有参数的函数
    /// </summary>
    public EventHandler m_EventSet;
    private Filter m_Filter;
    private T m_CurrentValue;
    private T m_LastValue;
    private T m_BaseValue;
    private T m_tempValue;
    public T m_initialValue;
    /// <summary>
    /// 构造函数，初始化 当前值 和 之前的值
    /// </summary>
    public Value(T initialValue)
    {
        m_initialValue = initialValue;
        m_CurrentValue = initialValue;
        m_LastValue = m_CurrentValue;
        m_BaseValue = initialValue;
        m_tempValue = initialValue;
    }
    /// <summary>
    /// 判断当前值是否等于给定值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Is(T value)
    {
        return m_CurrentValue != null && m_CurrentValue.Equals(value);
    }

    /// <summary>
    /// When this value will change, the callback method will be called.
    /// 给该数值的改变行为 添加监听者
    /// </summary>
    public void AddChangeListener(Action callback)
    {
        m_Set += callback;
    }

    public void AddChangeEventListener(EventHandler callback)
    {
        m_EventSet = callback;
    }
    /// <summary>
    /// 删除监听者
    /// </summary>
    public void RemoveChangeListener(Action callback)
    {
        m_Set -= callback;
    }

   public void RemoveChangeEventListener(EventHandler callback)
   {
       m_EventSet -= callback;
   }

    /// <summary>
    /// A "filter" will be called before the regular callbacks, useful for clamping values (like the player health, etc).
    /// 过滤器将在回调（Action m_Set）触发之前执行，该回调会在数值进行设置后触发
    /// 传进去一个函数，该函数通常用来对数值进行一个限制，比如角色的生命值
    /// 传进去表示这个限制的具体方法（过滤器）将会在外界进行编写，然后设置给这个Value
    /// </summary>
    public void SetFilter(Filter filter)
    {
        m_Filter = filter;
    }

    /// <summary>
    /// 获取 当前值
    /// </summary>
    public T Get()
    {
        return m_CurrentValue;
    }

    /// <summary>
    /// 获取 之前的值
    /// </summary>
    public T GetLastValue()
    {
        return m_LastValue;
    }
    /// <summary>
    /// 获取初始化数值
    /// </summary>
    /// <returns></returns>
    public T GetBaseValue()
    {
        return m_BaseValue;
    }

    public void SetBaseValue(T value)
    {
        m_BaseValue = value;
        m_CurrentValue = m_BaseValue;
        if (m_Set != null)
            m_Set();
    }
    public T GetTempValue()
    {
        return m_tempValue;
    }

    public void SetTempValue(T value)
    {
        m_tempValue = value;
        m_CurrentValue = value;
        if (m_Set != null)
            m_Set();
    }

    /// <summary>
    /// 修改当前的值 同时把 修改之前 当前的值 变为 之前的值
    /// 同时触发过滤器回调，接着触发修改数值的触发
    /// </summary>
    public void Set(T value)
    {
        m_LastValue = m_CurrentValue;
        m_CurrentValue = value;
        //Filter返回一个限制后合理的值给CurrentValue
        if (m_Filter != null)
            m_CurrentValue = m_Filter(m_LastValue, m_CurrentValue);
        //下面标明数值的设置不是第一次才能触发回调函数
        if (m_Set != null && (m_LastValue == null || !m_LastValue.Equals(m_CurrentValue)))
            m_Set();
    }
    /// <summary>
    /// 在改变值的时候附加一些其他参数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void SetWithArgs(T value,object sender, EventArgs args)
    {
        m_LastValue = m_CurrentValue;
        m_CurrentValue = value;
        //Filter返回一个限制后合理的值给CurrentValue
        if (m_Filter != null)
            m_CurrentValue = m_Filter(m_LastValue, m_CurrentValue);
        //下面标明数值的设置不是第一次才能触发回调函数
        if (m_EventSet != null && (m_LastValue == null || !m_LastValue.Equals(m_CurrentValue)))
            m_EventSet(sender, args);
    }
    /// <summary>
    /// 修改当前的值，同时强制更新
    /// </summary>
    public void SetAndForceUpdate(T value)
    {
        m_LastValue = m_CurrentValue;
        m_CurrentValue = value;

        if (m_Filter != null)
            m_CurrentValue = m_Filter(m_LastValue, m_CurrentValue);

        if (m_Set != null)
            m_Set();
    }

    public void SetAndDontUpdate(T value)
    {
        m_LastValue = m_CurrentValue;
        m_CurrentValue = value;

        if (m_Filter != null)
            m_CurrentValue = m_Filter(m_LastValue, m_CurrentValue);
    }
}
