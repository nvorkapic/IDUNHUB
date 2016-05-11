﻿using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace SenseHat
{
    public class LPS25H : Sensor
    {
        private const byte C_Addr = 0x5C;

        private const byte C_ResConf = 0x10;
        private const byte C_Ctrl1 = 0x20;
        private const byte C_Ctrl2 = 0x21;
        private const byte C_Status = 0x27;
        private const byte C_PressOutXL = 0x28;
        private const byte C_FifoCtrl = 0x2E;

        private const float pressureFactor = 1.0f / 4096.0f;

        protected override async Task InitAsync()
        {
            device = await I2cUtils.GetDeviceAsync(C_Addr, I2cBusSpeed.FastMode);

            // PD ODR2-0 DIFF BDU RESET SIM
            //  1    100    0   1     0   0
            // Active mode, 25Hz, default, non-continous, disable, default
            I2cUtils.WriteByte(device, C_Ctrl1, 0xC4);

            // AVGP1-0 AVGT1-0
            //      01      01
            // N. internal average: 32, 16 (pressure, temperature)
            I2cUtils.WriteByte(device, C_ResConf, 0x05);

            // F_MODE2 F_MODE1 F_MODE0
            //       1       1       0
            // FIFO_MEAN_MODE: running average filtered pressure
            I2cUtils.WriteByte(device, C_FifoCtrl, 0xC0);

            // BOOT FIFO_EN WTM_EN FIFO_MEAN I2C_EN SWRESET AUTOZERO ONESHOT
            //    0    1         0         0      0       0        0       0
            // normal, enable, disable, disable, i2c enable, normal, normal, waiting
            I2cUtils.WriteByte(device, C_Ctrl2, 0x40);
        }

        public float ReadPressure()
        {
            var status = I2cUtils.Read8(device, C_Status);
            if ((status & 2) == 2)
            {
                Int32 rawData = (Int32)I2cUtils.Read24LE(device, C_PressOutXL + 0x80);
                return rawData * pressureFactor;
            }
            return 0.0f;
        }
    }
}
