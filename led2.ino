#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 32
#define OLED_ADDR 0x3C
#define TCA_ADDR 0x70

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire);

String incomingData = "";

// Vybere kanál 1–2 na multiplexeru (TCA9548A)
void tcaSelect(uint8_t channel) {
  if (channel > 7) return;
  Wire.beginTransmission(TCA_ADDR);
  Wire.write(1 << channel);
  Wire.endTransmission();
}

void setup() {
  Serial.begin(9600);
  Wire.begin();
  Wire.setClock(400000);

  // Inicializuj displeje 2 a 3 (kanály 1 a 2)
  for (uint8_t i = 1; i <= 2; i++) {
    tcaSelect(i);
    if (!display.begin(SSD1306_SWITCHCAPVCC, OLED_ADDR)) {
      Serial.print("Displej na kanálu "); Serial.print(i); Serial.println(" nenalezen.");
    } else {
      display.clearDisplay();
      display.setTextSize(2);
      display.setTextColor(SSD1306_WHITE);
      display.setCursor(0, 10);
      display.print("D");
      display.print(i + 1); // Displej 2 a 3
      display.display();
    }
  }
}

void drawTemperature(String text) {
  display.clearDisplay();
  display.setTextSize(4);
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);

  // Najdi pozici "oC" nebo "°C" a vykresli ručně
  int pos = text.indexOf("oC");
  if (pos == -1) pos = text.indexOf("°C");

  if (pos != -1) {
    String number = text.substring(0, pos); // např. "23"
    display.print(number);

    // Získej aktuální pozici kurzoru po čísle
    int16_t x = display.getCursorX();
    int16_t y = display.getCursorY();

    // Nakresli malý kruh jako znak °
    display.fillCircle(x + 2, y + 4, 4, SSD1306_WHITE);

    // Posuň kurzor a vypiš "C"
    display.setCursor(x + 10, y);
    display.print("C");
  } else {
    // Pokud není °C v textu, vypiš vše
    display.print(text);
  }

  display.display();
}

void loop() {
  while (Serial.available() > 0) {
    char c = Serial.read();
    if (c == '\n') {
      // Rozdělení vstupu podle středníků
      String parts[2] = {"", ""};
      int index = 0;
      int start = 0;
      for (int i = 0; i <= incomingData.length() && index < 2; i++) {
        if (incomingData[i] == ';' || i == incomingData.length()) {
          parts[index++] = incomingData.substring(start, i);
          start = i + 1;
        }
      }

      // Displej 2 (kanál 1)
      tcaSelect(1);
      drawTemperature(parts[0]);

      // Displej 3 (kanál 2)
      tcaSelect(2);
      drawTemperature(parts[1]);

      incomingData = "";
    } else {
      incomingData += c;
    }
  }
}
