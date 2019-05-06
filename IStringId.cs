using System;

/// <summary>
/// 定义支持String类型的Id属性
/// </summary>
public interface IStringId
{
    /// <summary>
    /// 获取或设置唯一标识
    /// </summary>
    string Id { get; set; }
}
