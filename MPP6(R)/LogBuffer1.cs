using System;
using System.Collections.Concurrent;
using System.IO;
using System.Timers;

//Задание 6
/*Создать класс на языке C#, который:
Создать класс LogBuffer, который:
-представляет собой журнал строковых сообщений;
-предоставляет метод public void Add(string item);
-буферизирует добавляемые одиночные сообщения и записывает
их пачками в конец текстового файла на диске;
-периодически выполняет запись накопленных сообщений, когда
их количество достигает заданного предела;
-периодически выполняет запись накопленных сообщений по
истечение заданного интервала времени (вне зависимости от
наполнения буфера);
-выполняет запись накопленных сообщений асинхронно с
добавлением сообщений в буфер;*/

namespace SixthTask
{
    class LogBuffer
    {
        private static ConcurrentQueue<string> Messages = new ConcurrentQueue<string>();//одноаправленный связанны список
        private readonly StreamWriter _streamWriter;//запись в файл

        private const int TimeTarget = 1;
        private static readonly Timer timer = new Timer(TimeTarget);
        private const int MaxNum = 28;

        public LogBuffer(string filePath = "D:\\UNIVERSITY\\osisp2\\logbuffer.txt")
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File doesn't exists: " + filePath);
            }

            _streamWriter = new StreamWriter(filePath, true);
            timer.Elapsed += CheckTime;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void CheckTime(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(true);
            while (!Messages.IsEmpty)
            {
                Messages.TryDequeue(out var message);
                if (message != null)
                {
                    _streamWriter.WriteLineAsync(message);

                }
            }
        }

        private void CheckCapacity()
        {
            if (Messages.Count < MaxNum)
            {
                return;
            }

            while (!Messages.IsEmpty)
            {
                Messages.TryDequeue(out string message);
                if (message != null)
                {
                    _streamWriter.WriteLineAsync(message);
                }
            }
        }

        public void Add(string item)
        {
            Messages.Enqueue(item);
            CheckCapacity();
        }

        public void Close()
        {
            _streamWriter.Close();
        }
    }
}