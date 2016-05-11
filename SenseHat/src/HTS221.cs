﻿using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace SenseHat
{
    public class HTS221 : Sensor
    {
        public const byte C_Addr = 0x5F;

        public const byte C_AvConf = 0x10;
        public const byte C_Ctrl1 = 0x20;
        public const byte C_Status = 0x27;
        public const byte C_HumidityOutL = 0x28;
        public const byte C_HumidityOutH = 0x29;
        public const byte C_TempOutL = 0x2A;
        public const byte C_TempOutH = 0x2B;
        public const byte C_H0H2 = 0x30;
        public const byte C_H1H2 = 0x31;
        public const byte C_T0C8 = 0x32;
        public const byte C_T1C8 = 0x33;
        public const byte C_T1T0 = 0x35;
        public const byte C_H0T0Out = 0x36;
        public const byte C_H1T0Out = 0x3A;
        public const byte C_T0Out = 0x3C;
        public const byte C_T1Out = 0x3E;

        private Func<Int16, float> convertTemperature;
        private Func<Int16, float> convertHumidity;

        private Func<Int16, float> GetTemperatureConverter()
        {
            byte rawMsb = I2cUtils.Read8(device, C_T1T0 + 0x80);

            byte t0Lsb = I2cUtils.Read8(device, C_T0C8 + 0x80);
            byte t1Lsb = I2cUtils.Read8(device, C_T1C8 + 0x80);

            UInt16 t0c8 = (UInt16)(((UInt16)(rawMsb & 0x03) << 8) | (UInt16)t0Lsb);
            UInt16 t1c8 = (UInt16)(((UInt16)(rawMsb & 0x0C) << 6) | (UInt16)t1Lsb);

            float t0 = t0c8 / 8.0f;
            float t1 = t1c8 / 8.0f;

            Int16 t0out = (Int16)I2cUtils.Read16LE(device, C_T0Out + 0x80);
            Int16 t1out = (Int16)I2cUtils.Read16LE(device, C_T1Out + 0x80);

            float m = (t1 - t0) / (t1out - t0out);
            float b = t0 - (m * t0out);

            return t => t * m + b;
        }

        private Func<Int16, float> GetHumidityConverter()
        {
            byte h0h2 = I2cUtils.Read8(device, C_H0H2 + 0x80);
            byte h1h2 = I2cUtils.Read8(device, C_H1H2 + 0x80);

            float h0 = h0h2 * 0.5f;
            float h1 = h1h2 * 0.5f;

            Int16 h0t0out = (Int16)I2cUtils.Read16LE(device, C_H0T0Out + 0x80);
            Int16 h1t0out = (Int16)I2cUtils.Read16LE(device, C_H1T0Out + 0x80);

            float m = (h1 - h0) / (h0t0out - h1t0out);
            float b = h0 - (m * h0t0out);

            return t => t * m + b;
        }

        protected override async Task InitAsync()
        {
            device = await I2cUtils.GetDeviceAsync(C_Addr, I2cBusSpeed.FastMode);

            // PD RESERVED BDU ODR1-0
            //  1     0000   1     11
            // active, n/a, non-continous, 12.5Hz for both
            I2cUtils.WriteByte(device, C_Ctrl1, 0x87);

            // RESERVED AVGT2-0 AVGH2-0
            //       00     011     011
            // n/a, average: 16, 32 (temperature, humidity)
            I2cUtils.WriteByte(device, C_AvConf, 0x1B);

            convertTemperature = GetTemperatureConverter();
            convertHumidity = GetHumidityConverter();
        }

        public float ReadTemperature()
        {
            var status = I2cUtils.Read8(device, C_Status);
            if ((status & 1) == 1)
            {
                var rawData = (Int16)I2cUtils.Read16LE(device, C_TempOutL + 0x80);
                return convertTemperature(rawData);
            }
            // TODO: throw exception or return status code if reading is ready or not?
            return 0.0f;
        }

        public float ReadHumidity()
        {
            var status = I2cUtils.Read8(device, C_Status);
            if ((status & 2) == 2)
            {
                var rawData = (Int16)I2cUtils.Read16LE(device, C_HumidityOutL + 0x80);
                return convertHumidity(rawData);;
            }
            // TODO: throw exception or return status code if reading is ready or not?
            return 0.0f;
        }
    }
}
