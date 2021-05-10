using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item  {
    [SerializeField]//物品名稱
    private string itemName;
    [SerializeField]//物品ID
    private string itemID;
    [SerializeField]//一堆有幾個物品(預設不可堆疊：一堆1個)
    private int stackCount = 1;
    public bool isStackable { get { return stackCount > 1; } }
    [SerializeField]//物品Icon
    private Sprite itemIcon;
    #region 外部接口(讀取用)
    public string name { get { return itemName; } }
    public string id { get { return itemID; } }
    public int amount { get; private set; }
    public int amount_divisor { get { return amount / stackCount; } }//幾堆
    public int amount_remainder { get { return amount % stackCount; } }//堆完剩下的
    public int stackLimit { get { return stackCount; } }//一堆的數量
    public Sprite icon { get { return itemIcon; } }
    #endregion
    public void UpdateAmount(int Count = 1)
    {
        amount += Count;
    }
    /// <summary>
    /// 複製資料(格式)給外部使用
    /// </summary>
    /// <returns>Item複製體</returns>
    public Item Clone()
    {
        return (Item)MemberwiseClone();
    }
}
