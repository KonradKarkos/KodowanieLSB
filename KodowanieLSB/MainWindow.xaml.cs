using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Collections;
using Color = System.Drawing.Color;

namespace KodowanieLSB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //uruchomienia okna dialogowego i wybranie pliku
        public void wybierz_plik(TextBox tab)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Obrazy bitmap (*.bmp)|*.bmp|Tagged Image File Format (*.tif)|*.tif|Joint Photographic Experts Group (*.jpg)|*.jpg|Graphic Interchange Format (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                tab.Text = openFileDialog.FileName;
            }
        }
        //wypełnienie wybranego pola z pomocą okna dialogowego i zaproponowanie przykładowej nazwy pliku w drugim
        private void WybierzPlikDoKodownia_Click(object sender, RoutedEventArgs e)
        {
            wybierz_plik(PlikBox);
            Plik2Box.Text = PlikBox.Text.Insert(PlikBox.Text.Length - 4, "Zakodowane");
        }

        private void WybierzPlikDoZapisania_Click(object sender, RoutedEventArgs e)
        {
            wybierz_plik(Plik2Box);
        }
        //wypełnienie wybranego pola z pomocą okna dialogowego i zaproponowanie przykładowej nazwy pliku w drugim
        private void WybierzPlikDoKodownia_Copy_Click(object sender, RoutedEventArgs e)
        {
            wybierz_plik(PlikBox_Copy);
            Plik2Box_Copy.Text = PlikBox_Copy.Text.Remove(PlikBox_Copy.Text.Length - 4, 4).Insert(PlikBox_Copy.Text.Length - 4, ".txt");
        }
        //okno dialogowe do wybrania pliku txt
        private void WybierzPlikDoZapisania_Copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Plik2Box_Copy.Text = openFileDialog.FileName;
            }
        }
        //wczytanie tekstu z podanego pliku txt
        private void WczytajTxT_Click(object sender, RoutedEventArgs e)
        {
            String Plik;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Plik = openFileDialog.FileName;
                StreamReader sr = new StreamReader(Plik);
                StringBuilder sb = new StringBuilder();
                String linia;
                while ((linia = sr.ReadLine()) != null) { sb.Append(linia); }
                TextBox.Text = sb.ToString();
                sr.Dispose();
            }
        }

        private void Koduj_Click(object sender, RoutedEventArgs e)
        {
            //sprawdzenie czy plik do zakodowania tekstu istnieje
            if (File.Exists(PlikBox.Text))
            {
                //przerobienie pliku na bitmapę
                Bitmap obraz = new Bitmap(PlikBox.Text);
                int dlugosc = TextBox.Text.Length;
                //sprawdzenie czy tekst zmieści się w podanym obrazie
                if (dlugosc * 8 > obraz.Height * obraz.Width * 6)
                {
                    MessageBox.Show("Wiadomość jest zbyt długa dla podanego obrazu!");
                }
                else
                {
                    Byte[] znaki = new Byte[dlugosc];
                    String DoZakodowania = TextBox.Text;
                    //przerobienie liter na formę w bajtach
                    for (int i = 0; i < dlugosc; i++)
                    {
                        znaki[i] = (byte)DoZakodowania[i];
                    }
                    //przerobienie bajtów na tablicę bitów
                    BitArray Bity = new BitArray(znaki);
                    dlugosc = dlugosc * 8;
                    int ilosc = obraz.Height * obraz.Width * 6;
                    int pozycja = 0;
                    int szerokosc = obraz.Width;
                    int wysokosc = obraz.Height;
                    Color c;
                    Bitmap Zakodowane = new Bitmap(szerokosc, wysokosc);
                    Color clr;
                    //tablica przechowująca informacje o kolorach
                    BitArray[] kolory = new BitArray[3];
                    //zmienna potrzebna do konwersji
                    Byte[] kolor = new Byte[1];
                    //ta też
                    int[] konwersja = new int[3];
                    for(int i=0;i<szerokosc;i++)
                    {
                        for(int j=0;j<wysokosc;j++)
                        {
                            //pobranie pixela do modyfikacji 
                            clr = obraz.GetPixel(i, j);
                            //domyślne przypisanie pobranego pixela do nowego
                            c = clr;
                            if(pozycja<dlugosc)
                            {
                                //zamiana składowych koloru na tablice bitowe w celu modyfikacji
                                kolor[0] = clr.R;
                                kolory[0] = new BitArray(kolor);
                                kolor[0] = clr.G;
                                kolory[1] = new BitArray(kolor);
                                kolor[0] = clr.B;
                                kolory[2] = new BitArray(kolor);
                                kolory[0][6] = Bity[pozycja];
                                pozycja++;
                                //warunki na wypadek gdyby nastąpił koniec ciągu żeby nie wyjść poza zakres
                                if (pozycja < dlugosc)
                                {
                                    kolory[0][7] = Bity[pozycja];
                                    pozycja++;
                                }
                                if (pozycja < dlugosc)
                                {
                                    kolory[1][6] = Bity[pozycja];
                                    pozycja++;
                                }
                                if (pozycja < dlugosc)
                                {
                                    kolory[1][7] = Bity[pozycja];
                                    pozycja++;
                                }
                                if (pozycja < dlugosc)
                                {
                                    kolory[2][6] = Bity[pozycja];
                                    pozycja++;
                                }
                                if (pozycja < dlugosc)
                                {
                                    kolory[2][7] = Bity[pozycja];
                                    pozycja++;
                                }
                                for(int z =0;z<3;z++)
                                {
                                    kolory[z].CopyTo(konwersja, z);
                                }
                                //przypisanie zakodowanego pixela do nowego
                                c = Color.FromArgb(konwersja[0], konwersja[1], konwersja[2]);
                            }
                            //ustawienie nowego pixela do w noweym obrazie
                            Zakodowane.SetPixel(i, j, c);
                        }
                    }
                    //zapisanie pliku i poinformowanie użytkownika o końcu procesu
                    Zakodowane.Save(Plik2Box.Text);
                    MessageBox.Show("Proces kodowana zakończony!");
                    obraz.Dispose();
                    Zakodowane.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Obraz wybrany do kodowania nie istnieje!");
            }
        }

        private void Dekoduj_Click(object sender, RoutedEventArgs e)
        {
            //sprawdzenien czy plik do zdekodowania istnieje
            if(File.Exists(PlikBox_Copy.Text))
            {
                Bitmap obraz = new Bitmap(PlikBox_Copy.Text);
                int szerokosc = obraz.Width;
                int wysokosc = obraz.Height;
                Color clr;
                BitArray[] kolory = new BitArray[3];
                BitArray znaki = new BitArray(szerokosc * wysokosc * 6);
                int pozycja = 0;
                Byte[] kolor = new Byte[1];
                //odczytywanie dwóch najmniej zaczących bitów z składowych kolorów każdego piksela
                for (int i=0;i<szerokosc;i++)
                {
                    for(int j=0;j<wysokosc;j++)
                    {
                        clr = obraz.GetPixel(i, j);
                        kolor[0] = clr.R;
                        kolory[0] = new BitArray(kolor);
                        kolor[0] = clr.G;
                        kolory[1] = new BitArray(kolor);
                        kolor[0] = clr.B;
                        kolory[2] = new BitArray(kolor);
                        znaki[pozycja] = kolory[0][6];
                        znaki[pozycja+1] = kolory[0][7];
                        znaki[pozycja+2] = kolory[1][6];
                        znaki[pozycja+3] = kolory[1][7];
                        znaki[pozycja+4] = kolory[2][6];
                        znaki[pozycja+5] = kolory[2][7];
                        pozycja += 6;
                    }
                }
                Byte[] LiteryWBajtach = new Byte[znaki.Length/8];
                BitArray Litera;
                //konwersja odczytanych bitów na litery w kodzie ASCII
                for(int i=0;i<LiteryWBajtach.Length;i++)
                {
                    Litera = new BitArray(new bool[] { znaki[i*8], znaki[i * 8+1], znaki[i * 8+2], znaki[i * 8+3], znaki[i * 8+4], znaki[i * 8+5], znaki[i * 8+6], znaki[i * 8+7] });
                    Litera.CopyTo(LiteryWBajtach, i);
                }
                TextBox_Copy.Text = Encoding.ASCII.GetString(LiteryWBajtach);
                //powiadomienie użytkownika o zakończeniu procesu
                MessageBox.Show("Dekodowanie zakończone!");
                obraz.Dispose();
            }
            else
            {
                MessageBox.Show("Plik podany do odkodowania nie istnieje!");
            }
        }

        private void ZapiszTxT_Click(object sender, RoutedEventArgs e)
        {
            //zapisanie tekstu do wskazanego pliku
            StreamWriter sw = new StreamWriter(Plik2Box_Copy.Text);
            if (TextBox_Copy.LineCount > 0)
            {
                for (int i = 0; i < TextBox_Copy.LineCount; i++)
                {
                    sw.Write(TextBox_Copy.GetLineText(i));
                }
                sw.Dispose();
            }
        }
        //metody mające na celu niedopuszczenie do rozpoczęcia akcji jeśli ścieżka pliku jest pusta
        private void PlikBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlikBox_Copy.Text.Length > 0) Dekoduj.IsEnabled = true;
            else Dekoduj.IsEnabled = false;
        }

        private void Plik2Box_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Plik2Box_Copy.Text.Length > 0) ZapiszTxT.IsEnabled = true;
            else ZapiszTxT.IsEnabled = false;
        }

        private void PlikBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlikBox.Text.Length > 0 && Plik2Box.Text.Length > 0) Koduj.IsEnabled = true;
            else Koduj.IsEnabled = false;
        }

        private void Plik2Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlikBox.Text.Length > 0 && Plik2Box.Text.Length > 0) Koduj.IsEnabled = true;
            else Koduj.IsEnabled = false;
        }

        private void HelpKod_Click(object sender, RoutedEventArgs e)
        {
            char c = (char)10;
            MessageBox.Show("");
        }
    }
}
