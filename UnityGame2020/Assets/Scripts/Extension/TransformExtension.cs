using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum Direction { Top, Down, Right, Left } //射擊方向 (上下左右)
public enum ShootType { Sector, Row } //射擊方式(扇形、橫排)
/// <summary>
/// 射擊方式相關的資訊(位置、角度、方式、紀錄)
/// </summary>
public struct ShootData
{
    public Vector3 pos { get; private set; }
    public Quaternion quat { get; private set; }
    public float ang { get; private set; }
    public int bowCount { get; private set;}
    public float bias;
    public bool isEven;
    public bool isCycle;
    public void SetValue(Vector3 pos, float ang, int bowCount = 0, bool isCycle = false)
    {
        this.pos = pos;
        this.ang = ang;
        this.bowCount = bowCount;
        this.isCycle = isCycle;
    }
    /// <summary>
    /// 計算四個方向(直線)的發射點
    /// </summary>
    /// <returns>發射點</returns>
    public Vector3 ShootPoint()
    {
        quat = Quaternion.AngleAxis(ang, Vector3.up);
        //面向正前方向量
        Vector3 frontVT = quat * (Vector3.forward * 1.3f);
        //用正上方與面向正前方向量的叉積求出面前右邊90度的向量
        Vector3 rightVT = Vector3.Cross(frontVT, Vector3.up) *
                (bias * 0.2f - (isEven ? Mathf.Sign(bias) * 0.1f : 0));
        //叉積*(偏移量-(如果子彈為偶數顆則偏移額外左右橫跳))
        //pos += Vector3.right*bias;
        return pos + frontVT + rightVT;
    }
    /// <summary>
    /// 計算弓形發射點
    /// </summary>
    /// <param name="angle">預設角度:360度(環狀)</param>
    /// <returns>發射點</returns>
    public Vector3 ShootPoint(float angle=360)
    {
        //如果angle(角度)為360，使用環狀
        float cellAng = (angle == 360) ? (360 / bowCount) : angle / (bowCount - 1);
        quat = Quaternion.AngleAxis(cellAng * bias + ang, Vector3.up);
        //前方向量
        Vector3 frontVT = quat * (Vector3.forward * 1.3f);
        return pos + frontVT;
    }
    /*
    public Vector3 ShootPoint(float angle=360)
    {
        float cellAng =angle / (bowCount - 1);
        quat = Quaternion.AngleAxis(cellAng * bias + ang, Vector3.up);
        //前方向量
        Vector3 frontVT = quat * (Vector3.forward * 1.3f);
        return pos + frontVT;
    }
    public Vector3 CycleShootPoint()
    {
        float cellAng = (360 / bowCount);
        quat = Quaternion.AngleAxis(cellAng * bias + ang, Vector3.up);
        //前方向量
        Vector3 frontVT = quat * (Vector3.forward * 1.3f);
        return pos + frontVT;
    }
    */
}
/// <summary>
/// 各式發射方式的發射數量
/// </summary>
public struct DirectionBullet
{
    public int Front;
    public int Side;
    public int Back;
    public int Bow;
    public int Cycle;
    public bool SideShoot { get { return Side > 0; } }
    public bool BackShoot { get { return Back > 0; } }
    public bool BowShoot { get { return Bow > 0; } }
    public bool CycleShoot { get { return Cycle > 0; } }
}
/// <summary>
/// 擴充方法
/// </summary>
public static class TransformExtension 
{
    public static List<ShootData> GetShootData(this Vector3 pos,float ang,DirectionBullet db)
    {
        List<ShootData> SDList = new List<ShootData>();
        ShootData SD = new ShootData();
        if(db.CycleShoot)
        {
            foreach (ShootData CSD in GetCycShootData(pos, ang, db, db.Cycle))
            {
                SDList.Add(CSD);
            }
        }
        if(db.BowShoot)
            for (int i = 2; i <= db.Bow * 2 + 1; i++)
            {
                SD.bias = ((i % 2) > 0 ? 1 : -1) * (i / 2);
                SD.SetValue(pos, ang, db.Bow * 2 + 1);
                SDList.Add(SD);
            }
        for (int i = 1; i <= db.Front+1; i++)
        {
            //D 確認子彈數是否為偶數
            SD.isEven = (db.Front+1) % 2 == 0;
            //是偶數則+1(為了對稱)
            int I = i + (SD.isEven ? 1 : 0);
            SD.bias = ((I % 2) > 0 ? 1 : -1) * (I / 2);
            SD.SetValue(pos, ang);
            SDList.Add(SD);
        }
        if (db.SideShoot)
        {
            for (int i = 1; i <= db.Side; i++)
            {
                //D 確認子彈數是否為偶數
                SD.isEven = db.Side % 2 == 0;
                //是偶數則+1(為了對稱)
                int I = i + (SD.isEven ? 1 : 0);
                SD.bias = ((I % 2) > 0 ? 1 : -1) * (I / 2);
                SD.SetValue(pos, ang + 90);
                SDList.Add(SD);
                SD.SetValue(pos, ang - 90);
                SDList.Add(SD);
            }
        }
        if (db.BackShoot)
        {
            SD.bias = 0;
            for (int i = 1; i <= db.Back; i++)
            {
                //D 確認子彈數是否為偶數
                SD.isEven = db.Back % 2 == 0;
                //是偶數則+1(為了對稱)
                int I = i + (SD.isEven ? 1 : 0);
                SD.bias = ((I % 2) > 0 ? 1 : -1) * (I / 2);
                SD.SetValue(pos, ang + 180);
                SDList.Add(SD);
            }
        }
        return SDList;
    }

    public static List<ShootData> GetCycShootData(this Vector3 pos, float ang, DirectionBullet db,int count)
    {
        List<ShootData> SDList = new List<ShootData>();
        ShootData SD = new ShootData();
        for (int i = 0; i <count; i++)
        {
            SD.bias = i;
            SD.SetValue(pos, ang, count,true);
            SDList.Add(SD);
        }
        return (SDList);
    }
    #region 測試(沒再用)
    public static Vector3 ShootPosition(this Transform originTransform,Direction direction,float dist=1)
    {

        Vector3 point = Vector3.zero;
        switch(direction)
        {
            case (Direction.Top):
                point = originTransform.forward* dist;
                break;
            case (Direction.Down):
                point = -originTransform.forward * dist;
                break;
            case (Direction.Left):
                point = -originTransform.right * dist;
                break;
            case (Direction.Right):
                point = originTransform.right * dist;
                break;
        }
        return originTransform.position + point;
    }

    public static Quaternion ShootRotation(this Transform originTransform, Direction direction, float ang=0)
    {
        Quaternion quaternion = Quaternion.identity;
        float angle = 0;
        switch (direction)
        {
            case (Direction.Top):
                angle = ang;
                break;
            case (Direction.Down):
                angle = ang+180;
                break;
            case (Direction.Left):
                angle = ang-90;
                break;
            case (Direction.Right):
                angle = ang+90;
                break;
        }
        quaternion = Quaternion.AngleAxis(angle, Vector3.up);
        return quaternion;
    }
    #endregion
}
