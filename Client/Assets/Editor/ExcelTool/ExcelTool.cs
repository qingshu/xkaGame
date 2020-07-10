/************************************************************************************
* Copyright(c) 2019 All Rights Reserved.
*创建人： lyp
*创建时间：2019-04-11 23:03:23
*版本：1.0
*设计意图：生成读表相关的各个类
* 1、根据各个表生成对应的数据表类
* 2、生成序列化表格数据类
* 3、生成生成序列化资源的工具类
* 4、生成读取序列化资源的类
*=====================================================================
*修改时间：
*修改人：
*修改原因：
*修改内容：
************************************************************************************/ 
using System.Collections.Generic;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using UnityEditor;

public class ExcelBuild : Editor
{
    [MenuItem("Tools/CreateExcelCodeTool")]
    public static void CreateExcelCode()
    {
        //判断路径是否存在
        if (!Directory.Exists(AppConst.excelsFolderPath))
        {
            LogManager.Instance.LogError("表格路径不存在，请检查！");
            return;
        }

        //获取所有excel文件路径
        DirectoryInfo directoryInfo = new DirectoryInfo(AppConst.excelsFolderPath);
        FileInfo[] files = directoryInfo.GetFiles("*.xlsx", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            //获取表格名字
            string tableName = files[i].Name.Replace(".xlsx", "");

            //生成表类
        }

        //生成DataManager.cs

        //生成CreateExcelDataAsset.cs代码
    }
}

public class ExcelTool
{   
    /// <summary>
    /// 读取表数据，生成对应的数组
    /// </summary>
    /// <param name="filePath">excel文件全路径</param>
    /// <returns>Item数组</returns>
    public static T[] CreateArrayWithExcel<T>(string filePath) where T: new()
    {
        //获得表数据
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

        //先获取所有的字段名和字段类型
        //fieldMap key:字段名 value:在表格中的列数
        //fieldTypeMap key:字段名 value:字段类型
        Dictionary<string, string> fieldMap = new Dictionary<string, string>();
        Dictionary<string, string> fieldTypeMap = new Dictionary<string, string>();
        for (int i = 0; i < columnNum; i++) {
            string fieldAndType = collect[1][i].ToString();
            string[] fieldAndTypes = fieldAndType.Split('|');
            if (fieldAndTypes.Length < 2) {
                LogManager.Instance.LogError("程序字段配置错误，表格：" + nameof(T) + "列：" + i + 1);
                return null; 
            }
            string type = fieldAndTypes[0];
            string field = fieldAndTypes[1];
            fieldTypeMap[field] = type;
            if (!fieldMap.ContainsKey(field))
            {
                fieldMap[field] = i.ToString();
            }
            else
            {
                fieldMap[field] += "|" + i;
            }
            
        }

        //反射为泛型数据赋值（根据excel的定义，第三行开始才是数据）
        T[] array = new T[rowNum - 2];
        for (int i = 2; i < rowNum; i++)
        {
            T item = new T();
            var type = item.GetType();
            var fields = type.GetFields();
            for (int j = 0; j < fields.Length; j++)
            {
                var field = fields[j];
                string valueIndex = fieldMap[field.Name];
                if (valueIndex == "") {
                    LogManager.Instance.LogError("不存在变量名的数据：" + field.Name + ",类名：" + type.Name);
                    continue;
                }
                string[] valueIndexs = valueIndex.Split('|');
                if (valueIndexs.Length > 1)
                {
                    //处理数组数据（相同字段有多个数据则表示数组）
                    string fieldType = fieldTypeMap[field.Name];                  
                    switch (fieldType)
                    {
                        //暂时没有好的方法获取类型，先特殊处理
                        case "int":
                            var arrayIntData = GetArrayData<int>(collect[i], fieldType, valueIndexs);
                            field.SetValue(item, arrayIntData);
                            break;
                        case "float":
                            var arrayFloatData = GetArrayData<float>(collect[i], fieldType, valueIndexs);
                            field.SetValue(item, arrayFloatData);
                            break;
                        case "string":
                            var arrayStringData = GetArrayData<string>(collect[i], fieldType, valueIndexs);
                            field.SetValue(item, arrayStringData);
                            break;
                        case "bool":
                            var arrayBoolData = GetArrayData<bool>(collect[i], fieldType, valueIndexs);
                            field.SetValue(item, arrayBoolData);
                            break;
                        default:
                            LogManager.Instance.LogError("表格中有没有实现的类型：" + fieldType);
                            return null;
                    }                  
                }
                else
                {
                    int index = int.Parse(valueIndex);
                    if (collect[i][index] == null)
                    {
                        continue;
                    }
                    field.SetValue(item, GetCeilValue(fieldTypeMap[field.Name],collect[i][index].ToString()));
                }
                
            }
            array[i - 2] = item;
        }
        return array;
    }

    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }

    /// <summary>
    /// 获取数组数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="row">一行数据</param>
    /// <param name="fieldType">表格定的数组数据类型</param>
    /// <param name="valueIndexs">这个数组分布在这一行的列索引</param>
    /// <returns></returns>
    static T[] GetArrayData<T>(DataRow row,string fieldType,string[] valueIndexs) 
    {
        var list = new List<T>();
        int valueCount = valueIndexs.Length;
        for (int k = 0; k < valueCount; k++)
        {
            if (valueIndexs[k] == "")
            {
                continue;
            }
            int index = int.Parse(valueIndexs[k]);
            if (row[index] == null)
            {
                continue;
            }
            list.Add((T)GetCeilValue(fieldType, row[index].ToString()));
        }
        return list.ToArray();
    }

    /// <summary>
    /// 根据类型返回对应的数据
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="value">数据</param>
    /// <returns></returns>
    static object GetCeilValue(string type,string value) {
        switch (type)
        {
            case "int": return int.Parse(value);
            case "float": return float.Parse(value);
            case "string": return value;
            case "bool": return bool.Parse(value);
            default:
                LogManager.Instance.LogError("表格中有没有实现的类型：" + type);
                return null;
        }
    }
}

