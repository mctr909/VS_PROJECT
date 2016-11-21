using System;
using System.IO;
using System.Data;

namespace RPGツクールネタ帳
{
    /// <summary>
    /// 項目ID
    /// </summary>
    public struct ColumnID
    {
        /// <summary>
        /// シーケンス番号
        /// </summary>
        public const string SeqNo = "SeqNo";

        /// <summary>
        /// キャラクター名
        /// </summary>
        public const string CharName = "CharName";

        /// <summary>
        /// 出身
        /// </summary>
        public const string Shusshin = "Shusshin";

        /// <summary>
        /// 職業
        /// </summary>
        public const string Shokugyou = "Shokugyou";

        /// <summary>
        /// 年齢
        /// </summary>
        public const string Nenrei = "Nenrei";

        /// <summary>
        /// 自由項目
        /// </summary>
        public const string JiyuuKoumoku = "JiyuuKoumoku";

        /// <summary>
        /// 顔グラファイル名
        /// </summary>
        public const string FaceGraphFile = "FaceGraphFile";

        /// <summary>
        /// キャラチップファイル名
        /// </summary>
        public const string CharChipFile = "CharChipFile";
    }

    /// <summary>
    /// 項目
    /// </summary>
    public struct Column
    {
        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int SeqNo;

        /// <summary>
        /// キャラクター名
        /// </summary>
        public string CharName;

        /// <summary>
        /// 出身
        /// </summary>
        public string Shusshin;

        /// <summary>
        /// 職業
        /// </summary>
        public string Shokugyou;

        /// <summary>
        /// 年齢
        /// </summary>
        public string Nenrei;

        /// <summary>
        /// 自由項目
        /// </summary>
        public string JiyuuKoumoku;

        /// <summary>
        /// 顔グラファイル名
        /// </summary>
        public string FaceGraphFile;

        /// <summary>
        /// キャラチップファイル名
        /// </summary>
        public string CharChipFile;
    }

    public class Record
    {
        private DataTable dt;
        
        public Record()
        {
            dt = new DataTable();
            dt.Columns.Add(ColumnID.SeqNo, Type.GetType("System.Int32"));
            dt.Columns.Add(ColumnID.CharName, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.Shusshin, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.Shokugyou, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.Nenrei, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.JiyuuKoumoku, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.FaceGraphFile, Type.GetType("System.String"));
            dt.Columns.Add(ColumnID.CharChipFile, Type.GetType("System.String"));
        }

        public void Read(string fileName)
        {

        }

        public void Save(Column col)
        {
            DataRow[] rows = dt.Select(ColumnID.SeqNo + " = " + col.SeqNo);
            DataRow row = (rows.Length == 0 ? dt.NewRow() : rows[0]);

            row[ColumnID.SeqNo] = col.SeqNo;
            row[ColumnID.CharName] = col.CharName;
            row[ColumnID.Shusshin] = col.Shusshin;
            row[ColumnID.Shokugyou] = col.Shokugyou;
            row[ColumnID.Nenrei] = col.Nenrei;
            row[ColumnID.JiyuuKoumoku] = col.JiyuuKoumoku;
            row[ColumnID.FaceGraphFile] = col.FaceGraphFile;
            row[ColumnID.CharChipFile] = col.CharChipFile;

            if (rows.Length == 0)
            {
                dt.Rows.Add(row);
            }
        }
    }
}
