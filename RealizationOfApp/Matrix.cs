using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealizationOfApp
{

    using System.Text;
    public class Matrix : ICloneable
    {
        public float this[int index1, int index2]
        {
            get
            {
                if (matr.Count==0)
                    throw new Exception("Matrix was Empty");
                if (index1<0 || index1>=matr.Count)
                    throw new ArgumentException("Index1 was out of bounds");
                if (index2<0 || index2>=matr[0].Count)
                    throw new ArgumentException("Index2 was out of bounds");
                return matr[index1][index2];
            }
            set
            {
                if (matr.Count==0)
                    throw new Exception("Matrix was Empty");
                if (index1<0 || index1>=matr.Count)
                    throw new ArgumentException("Index1 was out of bounds");
                if (index2<0 || index2>=matr[0].Count)
                    throw new ArgumentException("Index2 was out of bounds");
                matr[index1][index2]=MaxResult(value);
            }
        }
        public int CountOfStrings { get => matr.Count; }
        public int CountOfCollums
        {
            get
            {
                if (matr.Count==0)
                    throw new Exception("Matrix was empty");
                return matr[0].Count;
            }
        }
        protected List<List<float>> matr = new();
        public Func<float, float> MaxResult = x => x;
        public Matrix()
        {
            matr = new();
        }
        public Matrix(in IEnumerable<IEnumerable<float>> matr)
        {
            this.matr=new(matr.Count());
            foreach (IEnumerable<float> floats in matr)
            {
                this.matr.Add(new List<float>(floats));
            }
        }
        public Matrix(in Matrix matrix)
        {
            matr=new(matrix.matr.Count);
            foreach (IEnumerable<float> floats in matrix.matr)
            {
                matr.Add(new List<float>(floats));
            }
        }
        public Matrix(int CountOfCollums, int CountOfStrings)
        {
            for (int i = 0; i<CountOfStrings; ++i)
            {
                matr.Add(new List<float>(new float[CountOfCollums]));
            }
        }
        #region Adds
        public void AddLastString(IEnumerable<float> floats)
        {
            if (matr.Count!=0 &&  floats.Count()!=matr[0].Count)
                throw new Exception("String size is non appropriate");
            matr.Add(new List<float>(floats));
        }
        public void AddLastCollum(IEnumerable<float> floats)
        {
            if (matr.Count!=0 && floats.Count()!=matr.Count)
                throw new Exception("Collum size is non appropriate");
            IEnumerator<float> enumerator = floats.GetEnumerator();
            enumerator.MoveNext();
            if (matr.Count==0)
            {
                for (int i = 0; i<floats.Count(); ++i)
                {
                    matr.Add(new List<float>(1));
                }
            }
            for (int i = 0; i<matr.Count; ++i)
            {
                matr[i].Add(enumerator.Current);
                enumerator.MoveNext();
            }
        }
        public void AddFrontString(IEnumerable<float> floats)
        {
            if (matr.Count!=0 && floats.Count()!=matr[0].Count)
                throw new Exception("String size is non appropriate");
            matr.Insert(0, new List<float>(floats));
        }
        public void AddFrontCollum(IEnumerable<float> floats)
        {
            if (floats.Count()!=matr.Count && matr.Count!=0)
                throw new Exception("Collum size is non appropriate");
            IEnumerator<float> enumerator = floats.GetEnumerator();
            enumerator.MoveNext();
            if (matr.Count==0)
            {
                for (float i = 0; i<floats.Count(); ++i)
                {
                    matr.Add(new List<float>(1));
                }
            }
            for (int i = 0; i<matr.Count; ++i)
            {
                matr[i].Insert(0, enumerator.Current);
                enumerator.MoveNext();
            }
        }
        public void AddString(int index, IEnumerable<float> floats)
        {
            if (index<0 || index>=matr.Count && matr.Count!=0)
                throw new ArgumentException("Index was out of bounds");
            if (matr.Count!=0 && floats.Count()!=matr[0].Count)
                throw new Exception("String size is non appropriate");
            index = matr.Count==0 ? 0 : index;
            matr.Insert(index, new List<float>(floats));
        }
        public void AddCollum(int index, IEnumerable<float> floats)
        {
            if (index<0 || matr.Count!=0 && index>=matr[0].Count)
                throw new ArgumentException("Index2 was out of bounds");
            if (matr.Count!=0 && floats.Count()!=matr.Count)
                throw new Exception("Collum size is non appropriate");
            IEnumerator<float> enumerator = floats.GetEnumerator();
            enumerator.MoveNext();
            if (matr.Count==0)
            {
                index = 0;
                for (int i = 0; i<floats.Count(); ++i)
                {
                    matr.Add(new List<float>(1));
                }
            }
            for (int i = 0; i<matr.Count; ++i)
            {
                matr[i].Insert(index, enumerator.Current);
                enumerator.MoveNext();
            }
        }
        #endregion
        public void DeleteString(int index)
        {
            if (index<0 || index>=matr.Count)
                throw new ArgumentException("Index was out of range");
            matr.RemoveAt(index);
        }
        public void DeleteCollum(int index)
        {
            if (matr.Count==0)
                throw new Exception("Matrix was empty");
            if (index<0 || index>=matr[0].Count)
                throw new ArgumentException("Index was out of range");
            foreach (List<float> list in matr)
            {
                list.RemoveAt(index);
            }
        }
        public bool ContainsItem(float item)
        {
            bool flag = false;
            for (int i = 0; i<matr.Count && !flag; ++i)
            {
                flag = matr[i].Contains(item);
            }
            return flag;
        }
        #region Indexers
        public int IndexOfItemString(float item)
        {
            int flag = -1;
            for (int i = 0; i<matr.Count && flag==-1; ++i)
            {
                flag = matr[i].Contains(item) ? i : -1;
            }
            return flag;
        }
        public int IndexOfItemCollum(float item)
        {
            int flag = -1;
            for (int i = 0; i<matr.Count && flag==-1; ++i)
            {
                flag = matr[i].Contains(item) ? matr[i].IndexOf(item) : -1;
            }
            return flag;
        }
        public int IndexOfString(Predicate<IEnumerable<float>> predicate)
        {
            int index = -1;
            for (int i = 0; i<matr.Count && index==-1; ++i)
            {
                index = predicate(matr[i]) ? i : -1;
            }
            return index;
        }
        public int IndexOfCollum(Predicate<IEnumerable<float>> predicate)
        {
            int index = -1;
            for (int i = 0; i<CountOfCollums && index==-1; ++i)
            {
                index = predicate(GetCollum(i)) ? i : -1;
            }
            return index;
        }
        #endregion
        public Matrix Transpose()
        {
            List<List<float>> list = new();
            for (int i = 0; i<CountOfCollums; ++i)
            {
                list.Add(new List<float>(GetCollum(i)));
            }
            return new Matrix(list);
        }
        public void Clear()
        {
            matr = new();
        }
        public object Clone()
        {
            return new Matrix(matr);
        }
        public static Matrix operator *(in Matrix matrix1, in Matrix matrix2)
        {
            if (matrix1.CountOfCollums!=matrix2.CountOfStrings)
                throw new Exception("Non-conformable matrices in operator *");
            List<List<float>> matrix = new(matrix1.CountOfStrings);
            for (int i = 0; i<matrix1.CountOfStrings; ++i)
            {
                matrix.Add(new List<float>());
                for (int j = 0; j<matrix2.CountOfCollums; ++j)
                {
                    matrix[i].Add(0);
                    for (int k = 0; k < matrix1.CountOfCollums; ++k)
                    {
                        matrix[i][j] = matrix1.MaxResult(matrix[i][j] + matrix1[i, k] * matrix2[k, j]);
                    }
                }
            }
            var matr = new Matrix(matrix);
            matr.MaxResult=matrix1.MaxResult;
            return matr;
        }
        public static Matrix operator +(in Matrix matrix1, in Matrix matrix2)
        {
            if (matrix1.CountOfStrings!=matrix2.CountOfStrings || matrix1.CountOfCollums!=matrix2.CountOfCollums)
                throw new Exception("Non-conformable matrices in operator +");
            List<List<float>> matrix = new(matrix1.CountOfStrings);
            for (int i = 0; i<matrix1.CountOfStrings; ++i)
            {
                matrix.Add(new List<float>(matrix1.CountOfCollums));
                for (int j = 0; j<matrix1.CountOfCollums; ++j)
                {
                    matrix[i].Add(matrix1.MaxResult(matrix1[i, j]+matrix2[i, j]));
                }
            }
            var matr = new Matrix(matrix)
            {
                MaxResult=matrix1.MaxResult
            };
            return matr;
        }
        public void RaiseToZero()
        {
            for (int i = 0; i<CountOfStrings; ++i)
            {
                for (int j = 0; j<CountOfCollums; ++j)
                {
                    matr[i][j]=i==j ? 1 : 0;
                }
            }
        }
        public void ForAll(Func<int, int, float, float> func)
        {
            for (int i = 0; i<CountOfStrings; ++i)
            {
                for (int j = 0; j<CountOfCollums; ++j)
                {
                    matr[i][j] = func(i, j, matr[i][j]);
                }
            }
        }
        public Matrix MultiplyByElem(Matrix matrix)
        {
            if (matrix.CountOfCollums!=CountOfCollums || matrix.CountOfStrings!=CountOfStrings)
                throw new Exception("For elem multiply need equal size matrixs");
            List<List<float>> floats = new();
            for (int i = 0; i<CountOfStrings; ++i)
            {
                floats.Add(new List<float>());
                for (int j = 0; j<CountOfCollums; ++j)
                {
                    floats[i].Add(MaxResult(matrix[i, j]*this[i, j]));
                }
            }
            var matr = new Matrix(floats);
            matr.MaxResult=matrix.MaxResult;
            return matr;
        }
        public float[,]? GetMatrixMassive()
        {
            float[,]? matrix = matr.Count>0 ? new float[matr.Count, matr[0].Count] : null;
            if (matr.Count!=0 && matrix is not null)
            {
                for (int i = 0; i<matr.Count; ++i)
                {
                    for (int j = 0; j<matr[0].Count; ++j)
                    {
                        matrix[i, j]=matr[i][j];
                    }
                }
            }
            return matrix;
        }
        public float[] GetString(int index)
        {
            if (index<0 || index>=matr.Count)
                throw new ArgumentException("Index was out of bounds");
            return matr[index].ToArray();
        }
        public float[] GetCollum(int index)
        {
            if (matr.Count==0)
                throw new Exception("Matrix was empty");
            if (index<0 || index>=matr[0].Count)
                throw new ArgumentException("Index2 was out of bounds");
            List<float> collum = new();
            for (int j = 0; j<CountOfStrings; ++j)
            {
                collum.Add(matr[j][index]);
            }
            return collum.ToArray();
        }
        public float FindCollum(Func<int, int, float, bool> predicate)
        {
            int index = -1;
            bool flag = true;
            for (int i = 0; i<matr[0].Count; ++i)
            {
                for (int j = 0; j<matr.Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[j][i]);
                }
                if (flag)
                {
                    index = i;
                    break;
                }
                flag = true;
            }
            return index;
        }
        public float FindLastCollum(Func<int, int, float, bool> predicate)
        {
            int index = -1;
            bool flag = true;
            for (int i = matr[0].Count-1; i>=0; --i)
            {
                for (int j = 0; j<matr.Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[j][i]);
                }
                if (flag)
                {
                    index = i;
                    break;
                }
                flag = true;
            }
            return index;
        }
        public IEnumerable<float> FindAllCollums(Func<int, int, float, bool> predicate)
        {
            List<float> collums = new();
            bool flag = true;
            for (int i = 0; i<matr[0].Count; ++i)
            {
                for (int j = 0; j<matr.Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[j][i]);
                }
                if (flag)
                {
                    collums.Add(i);
                }
                flag = true;
            }
            return collums;
        }
        public float FindString(Func<int, int, float, bool> predicate)
        {
            int index = -1;
            bool flag = true;
            for (int i = 0; i<matr.Count; ++i)
            {
                for (int j = 0; j<matr[i].Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[i][j]);
                }
                if (flag)
                {
                    index = i;
                    break;
                }
                flag = true;
            }
            return index;
        }
        public float FindLastString(Func<int, int, float, bool> predicate)
        {
            int index = -1;
            bool flag = true;
            for (int i = matr.Count-1; i>=0; --i)
            {
                for (int j = 0; j<matr[i].Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[i][j]);
                }
                if (flag)
                {
                    index = i;
                    break;
                }
                flag = true;
            }
            return index;
        }
        public IEnumerable<float> FindAllStrings(Func<int, int, float, bool> predicate)
        {
            List<float> strings = new();
            bool flag = true;
            for (int i = 0; i<matr.Count; ++i)
            {
                for (int j = 0; j<matr[i].Count && flag; ++j)
                {
                    flag = predicate(i, j, matr[i][j]);
                }
                if (flag)
                {
                    strings.Add(i);
                }
                flag = true;
            }
            return strings;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            foreach (List<float> floats in matr)
            {
                foreach (float i in floats)
                {
                    stringBuilder.Append(i + " ");
                }
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
        public bool Find(Func<int, int, float, bool> func)
        {
            bool flag = false;
            for (int i = 0; i<CountOfStrings && !flag; ++i)
            {
                for (int j = 0; j<CountOfCollums && !flag; ++j)
                {
                    flag = func(i, j, matr[i][j]);
                }
            }
            return flag;
        }
        public float Aggregation(Func<int, int, float, float> aggregator, float seed = 0)
        {
            for (int i = 0; i<matr.Count; ++i)
            {
                for (int j = 0; j<matr[i].Count; ++j)
                {
                    seed+=aggregator(i, j, matr[i][j]);
                }
            }
            return seed;
        }
        public float CountValueInString(int index, Predicate<float> pr)
        {
            if (index<0||index>=CountOfStrings)
                throw new ArgumentException("Index was out of range");
            return matr[index].FindAll(pr).Count;
        }
    }





}
