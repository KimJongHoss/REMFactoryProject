using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Analysis;

namespace REMFactoryProject
{
    public partial class MainWindow : Window
    {
        public async Task getPanel2Data()
        {
            // Define data path
            var dataPath = Path.GetFullPath(@"제주특별자치도개발공사_제주삼다수공장 시간별 전력사용량_20230930.csv");

            // Load the data into the data frame
            var df = DataFrame.LoadCsv(dataPath);

            var dateGroupedData = new Dictionary<DateTime, List<List<object>>>();

            foreach (var row in df.Rows)
            {
                var date = (DateTime)row[0]; // 첫 번째 컬럼이 날짜 컬럼이라고 가정
                var rowData = new List<object>();

                foreach (var cell in row.Skip(1)) // 날짜 컬럼을 제외한 나머지 셀
                {
                    rowData.Add(cell);
                }

                // 날짜별로 리스트에 추가
                if (!dateGroupedData.ContainsKey(date))
                {
                    dateGroupedData[date] = new List<List<object>>();
                }
                dateGroupedData[date].Add(rowData);
            }

            // 결과 출력
            foreach (var date in dateGroupedData.Keys)
            {
                Console.WriteLine($"Date: {date.ToShortDateString()}");
                foreach (var row in dateGroupedData[date])
                {
                    foreach (var value in row)
                    {
                        // UI 스레드에서 Label 업데이트
                        Dispatcher.Invoke(() =>
                        {
                            labelLine1.Content = value.ToString();
                            labelLine2.Content = value.ToString();
                            labelLine3.Content = value.ToString();
                            sliderLine1.Value = (double)value;
                            sliderLine2.Value = (double)value;
                            sliderLine3.Value = (double)value;
                        });

                        // 20ms 대기
                        await Task.Delay(1000);
                    }
                }
            }
        }
       
    }

}
