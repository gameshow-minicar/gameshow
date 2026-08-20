//g++ yomitori.cpp -o yomitori
//  yomitori.cpp
//  
//
//  Created by x25066xx on 2026/08/17.
//

#include <iostream>
#include <fstream>
#include <string>
#include <fcntl.h>
#include <unistd.h>
#include <termios.h>

#define SERIAL_PORT "/dev/cu.M5StickC_Plus_IMU"
#define FILE_NAME "./data.txt"

int main()
{
    // シリアルポートを開く
    int serial = open(SERIAL_PORT, O_RDWR | O_NOCTTY);
    
    if (serial == -1)
    {
        std::cerr << "M5StickCのシリアルポートを開けませんでした。" << std::endl;
        return 1;
    }
    
    // シリアル通信の設定
    struct termios tty;
    
    if (tcgetattr(serial, &tty) != 0)
    {
        std::cerr << "シリアル通信の設定を取得できませんでした。" << std::endl;
        close(serial);
        return 1;
    }
    
    // 115200bps
    cfsetispeed(&tty, B115200);
    cfsetospeed(&tty, B115200);
    
    // 8bit
    tty.c_cflag &= ~CSIZE;
    tty.c_cflag |= CS8;
    
    // パリティなし
    tty.c_cflag &= ~PARENB;
    
    // ストップビット1
    tty.c_cflag &= ~CSTOPB;
    
    // ハードウェアフロー制御なし
    tty.c_cflag &= ~CRTSCTS;
    
    // 通信を有効化
    tty.c_cflag |= CREAD | CLOCAL;
    
    // Canonical mode
    // 改行まで受信する
    tty.c_lflag |= ICANON;
    
    // エコーなどを無効化
    tty.c_lflag &= ~(ECHO | ECHOE | ECHONL);
    
    // 設定を反映
    tcsetattr(serial, TCSANOW, &tty);
    
    std::cout << "M5StickCに接続しました。" << std::endl;
    std::cout << "終了するには Ctrl + C を押してください。" << std::endl;
    
    while (true)
    {
        char buffer[256];
        
        int n = read(serial, buffer, sizeof(buffer) - 1);
        
        if (n > 0)
        {
            buffer[n] = '\0';
            
            std::string data(buffer);
            
            // 改行を削除
            if (!data.empty() && data.back() == '\n')
                data.pop_back();
            
            if (!data.empty() && data.back() == '\r')
                data.pop_back();
            
            // C++側でも確認
            std::cout << "受信: " << data << std::endl;
            
            // テキストファイルを上書き
            std::ofstream file(FILE_NAME);
            
            if (file.is_open())
            {
                file << data << std::endl;
                file.close();
            }
            else
            {
                std::cerr << "data.txtを開けませんでした。" << std::endl;
            }
        }
    }
    
    close(serial);
    
    return 0;
}
