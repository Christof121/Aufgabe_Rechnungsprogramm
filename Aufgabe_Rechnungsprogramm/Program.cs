using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Aufgabe_Rechnungsprogramm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region UserSettings
            /*
             ###########################################################
                                    User Settings
            */

            //Debug Ausgabe in der Konsole aktivieren | default: false
            bool debug = true;

            //Sollen vorausgefüllte Datensätze generiert werden? | default: true
            bool generatePrefilledData = true;

            //Standart Werte um vorausgefüllte Lieferscheine zu erstellen.
            //bei generatePrefilledData false hat die folgende Einstellung keine Auswirkung
            string[] datensatzWerkzeuge = { "Hammer", "Bohrer", "Zange", "Akkuschrauber", "Schleifmaschine", "CNC", "Fräser", "Nägel", "Schrauben", "Schraubenschlüssel", "Meißel", "Säge", "Riegel", "Pinsel", "Winkelschleifer", "Multimeter", "Lötstation", "Leim", "Zangen", "Drehmaschine", "Bohrmaschine", "Bandsäge", "Schweißgerät", "Stichsäge", "Bauleiter", "Wasserwaage", "Tapeziermesser", "Schaufel", "Abbruchhammer", "Schleifpapier", "Ratschenschlüssel", "Baukran", "Schaufelbagger", "Kombizange", "Kreissäge", "Eisenfeile", "Fräse", "Schlagbohrmaschine", "Schraubenzieher", "Lötzinn", "Kabelschneider", "Baumarkt", "Rasenmäher", "Kettensäge", "Gartenschere", "Akkubohrer", "Schnitzmesser", "Werkzeugtasche", "Gabelschlüssel", "Lötkolben", "Drehmomentmesser", "Feilen", "Wasserpumpe" };

            //Anzahl der vorausgefüllten Lieferwscheine die generiert werden sollen | deafult: 10
            //bei generatePrefilledData false hat die folgende Einstellung keine Auswirkung
            int anzahlDatensaetzeGenerieren = 10;

            //Anzahl der Artikel pro Lieferschein | deafult: 3
            //bei generatePrefilledData false hat die folgende Einstellung keine Auswirkung
            int anzahlDatensaetzeGenerierenPositionen = 3;

            //Tage der Zahlungsfrist | default: 14
            int zahlungsfrist = 14;

            /*
                            DO NOT EDIT BELOW THIS LINE
            ###########################################################
             */
            #endregion UserSettings

            Console.Title = "Rechnungsverwaltung"; // Titel der Konsole setrzten

            #region initialisierung
            string[][] daten = new string[100][];

            int uiLieferscheinnummer = -1;

            // Deklaration der User Input Variablen
            string uiMenu = "";
            string uiArtikelBezeichnung = "";
            double uiArtikelEinzelpreis = 0;
            int uiArtikelMenge = 0;

            double rechnungsSumme = 0;

            int indexLieferschein = 0;
            int indexArtikel = 0;

            int counterLieferschein = 0;
            int counterLieferscheinPositionen = 0;

            Random rand = new Random();

            // Vorausgefüllte Lieferscheine erzeugen
            if (generatePrefilledData)
            {
                for (int i = 0; i < anzahlDatensaetzeGenerieren; i++)
                {
                    daten[i] = new string[100];
                    for (int k = 0; k < anzahlDatensaetzeGenerierenPositionen; k++)
                    {
                        string werkzeug = datensatzWerkzeuge[rand.Next(1, datensatzWerkzeuge.Length) - 1];
                        string werkzeugPreis = (rand.Next(100, 1001) / 100.0).ToString("F2");
                        string werkzeugMenge = (rand.Next(1, 10).ToString());

                        daten[i][k] = string.Join(";", werkzeug, werkzeugPreis, werkzeugMenge);
                        if (debug) Console.WriteLine($"[DEBUG] Datensatz {i} Position {k} generiert");
                    }
                }
                if (debug) Console.WriteLine($"[DEBUG] {anzahlDatensaetzeGenerieren} Datensätze mit je {anzahlDatensaetzeGenerierenPositionen} wurden generiert");
            }
            #endregion initialisierung
            // Eigentlcher Programmstart

            do
            {
                #region Menü
                //Ausgabe Menü
                Console.WriteLine("###########################");
                Console.WriteLine("--- Rechnungsverwaltung ---\n");

                Console.WriteLine(" 1 - Artikel hinzufügen"); //Artikel dem Lieferschein hinzufügen
                Console.WriteLine(" 2 - Lieferschein ausgeben"); //Aktuelle Artikel des Lieferscheines ausgeben
                Console.WriteLine(" 3 - Rechnung erstellen"); //Rechnung für den Lieferschein erstellen
                Console.WriteLine(" 4 - Ausgabe \"Alles\""); //Ausgabe der Erstellten Rechnungen
                Console.WriteLine("");
                Console.WriteLine(" h - Hilfe");
                Console.WriteLine(" x - Programm beenden");
                Console.WriteLine("###########################");
                Console.Write("\nIhre Auswahl: ");
                uiMenu = Console.ReadLine().ToLower();

                #endregion Menü

                Console.Clear();

                // Auswertung Menüauswahl
                switch (uiMenu)
                {
                    case "1": //Artikel hinzufügen
                        #region Artikel hinzufügen
                        Console.WriteLine("Artikel hinzufügen\n");
                        Console.WriteLine("Zu welchem Lieferschein möchten Sie einen Arikel hinzufügen?\n");
                        Console.WriteLine("Um einen neuen Lieferschein zu erstellen geben Sie \"0\" an.");

                        //Abfrage Lieferscheinnummer
                        do
                        {
                            Console.Write("\n\nLieferscheinnummer: ");
                            if (int.TryParse(Console.ReadLine(), out uiLieferscheinnummer) && uiLieferscheinnummer >= 0)
                            {
                                //Eingabe gültig

                                if (uiLieferscheinnummer == 0) //Neuen Lieferschein erstellen
                                {
                                    indexLieferschein = daten.Count(e => e != null); // Zähle Datensetzte, Neue Lieferscheinnummer + 1
                                    if (debug) Console.WriteLine($"[DEBUG] Neuen Lieferschein erstellen, Erstelle Lieferschein ID {indexLieferschein + 1} | Array ID {indexLieferschein}");
                                    daten[indexLieferschein] = new string[100]; // Füge dem Array bis zu 100 Artikeleinträge hinzu
                                    break;
                                }
                                else if (daten[uiLieferscheinnummer - 1] != null)//Lieferschein editieren
                                {
                                    indexLieferschein = uiLieferscheinnummer - 1;
                                    if (debug) Console.WriteLine($"[DEBUG] Lieferschein mit ID {indexLieferschein} vorhanden.");
                                    break;
                                }
                            }
                            Console.WriteLine($"Die Lieferscheinnummer ist fehlerhaft, bitte überprüfen Sie Ihre Eingabe.");
                        }
                        while (true);

                        //Eingabe Artikelbezeichnung
                        Console.Write("Bezeichnung:");
                        uiArtikelBezeichnung = Console.ReadLine();

                        //Eingabe Einzelpreis
                        do
                        {
                            Console.Write("Einzelpreis: ");
                            if (double.TryParse(Console.ReadLine(), out uiArtikelEinzelpreis)) // überprüfung ob der eingegebende Preis gültig ist
                            {
                                //Eingabe gültig
                                break;
                            }
                            Console.WriteLine($"Der Angegebende Preis ist Fehlerhaft, bitte überprüfen Sie Ihre Eingabe.");
                        }
                        while (true);

                        //Eingabe Stückzahl
                        do
                        {
                            Console.Write("Menge: ");
                            if (int.TryParse(Console.ReadLine(), out uiArtikelMenge)) // Überprüfung bo die eingegebende Menge gültig ist
                            {
                                //Eingabe gültig
                                break;
                            }
                            Console.WriteLine($"Die Angegebende Menge ist Fehlerhaft, bitte überprüfen Sie Ihre Eingabe.");
                        }
                        while (true);

                        // Ausgabe Artikel hinzugefügt
                        Console.WriteLine($"\n\nFolgender Artikel wurde dem Lieferschein Nr. {indexLieferschein + 1} hinzugefügt");
                        Console.WriteLine($"{uiArtikelBezeichnung} | {uiArtikelEinzelpreis} Eur/Stck. | {uiArtikelMenge} Stck.");

                        indexArtikel = daten[indexLieferschein].Count(e => e != null); // Zähle Datensetzte Artikel im Lieferschein

                        daten[indexLieferschein][indexArtikel] = string.Join(";", uiArtikelBezeichnung, uiArtikelEinzelpreis, uiArtikelMenge);
                        #endregion Artikel hinzufügen
                        break;

                    case "2": //Lieferschein ausgeben
                        #region Lieferschein ausgeben
                        //Überprüfung Lieferscheinnummer
                        do
                        {
                            Console.Write("Lieferscheinnummer: ");
                            if (int.TryParse(Console.ReadLine(), out uiLieferscheinnummer) && uiLieferscheinnummer >= 1 && uiLieferscheinnummer <= 100 && daten[uiLieferscheinnummer - 1] != null) //Prüfen ob Eingabe gültig, im korrekten Bereich und vorhanden ist
                            {
                                // Eingabe gültig, setzte Index auf eingegebende Lieferscheinnummer -1 (Array startet mit zählung von 0 => 1 entspricht im Array 0)
                                indexLieferschein = uiLieferscheinnummer - 1;
                                break;
                            }
                            Console.WriteLine($"Die Lieferscheinnummer ist fehlerhaft, bitte überprüfen Sie Ihre Eingabe.");
                        } while (true);

                        counterLieferscheinPositionen = 1; //Counter für Artikelpositionen

                        //Ausgabe Lieferschein
                        Console.WriteLine("");
                        Console.WriteLine($"Lieferschein {uiLieferscheinnummer}\n");
                        Console.WriteLine($"{"Pos",-3} | {"Bezeichnung",-25} | {"Eur/Stck",-10} | {"Menge",-10}"); //{"Pos",-3} -3 gibt die Breite der Spalte an, positiver Wert = rechtsbündig, negativ = linksbündig
                        Console.WriteLine("_______________________________________________________________________");
                        foreach (var item in daten[indexLieferschein]) // Durchlüft alle Positionen für Lieferscheinnummer
                        {
                            if (item != null) // Überprüft ob die Position vorhanden ist
                            {
                                // Position vorhanden => Ausgabe
                                string[] fragments = item.Split(';');
                                //                          Pos                             Bezeichnung       Stückpreis           Menge
                                Console.WriteLine($"{counterLieferscheinPositionen,-3} | {fragments[0],-25} | {fragments[1],-10} | {fragments[2],-10}");
                                counterLieferscheinPositionen++;
                            };
                        }
                        #endregion Lieferschein ausgeben
                        break;

                    case "3": // Rechnung erstellen

                        #region Rechnung ausstellen

                        rechnungsSumme = 0; // Rechnungssumme auf 0 setzten

                        //Abfrage Lieferscheinnummer
                        do
                        {
                            Console.Write("Lieferscheinnummer: ");
                            if (int.TryParse(Console.ReadLine(), out uiLieferscheinnummer) && uiLieferscheinnummer >= 1 && uiLieferscheinnummer <= 100 && daten[uiLieferscheinnummer - 1] != null) //Prüfen ob Eingabe gültig, im korrekten Bereich und vorhanden ist
                            {
                                indexLieferschein = uiLieferscheinnummer - 1;
                                break;
                            }
                            Console.WriteLine($"Die Lieferscheinnummer ist fehlerhaft, bitte überprüfen Sie Ihre Eingabe.");
                        } while (true);

                        counterLieferscheinPositionen = 1; // Counter für Lieferscheinpositionen setzten
                        Console.WriteLine("");
                        Console.WriteLine($"Rechnung {uiLieferscheinnummer}\n");
                        // Ausgabe Inhalt des Lieferscheines
                        Console.WriteLine($"{"Pos",-3} | {"Bezeichnung",-25} | {"Eur/Stck",-10} | {"Menge",-10}");
                        Console.WriteLine("_______________________________________________________________________");

                        //Durchlaufe Alle Positionen im Lieferschein
                        foreach (var item in daten[indexLieferschein])
                        {
                            if (item != null) // Prüfung ob Position im Lieferschein vorhanden ist.
                            {
                                string[] fragments = item.Split(';');
                                //                          Pos                             Bezeichnung     Stückpreis      Menge
                                Console.WriteLine($"{counterLieferscheinPositionen,-3} | {fragments[0],-25} | {fragments[1],-10} | {fragments[2],-10}");
                                //Berechnung der Rechnungssumme
                                rechnungsSumme += Convert.ToDouble(fragments[2]) * Convert.ToDouble(fragments[1]); //Menge * Stückpreis
                                counterLieferscheinPositionen++;
                            };
                        }
                        //Rechnungszeile hinzufügen
                        Console.WriteLine("_______________________________________________________________________");
                        Console.WriteLine($"{"",-35}Summe: {rechnungsSumme} Eur"); // Ausgabe Rechnungssumme
                        Console.WriteLine($"\n{"",-35}Rechnung Zahlbar bis: {DateTime.Now.AddDays(zahlungsfrist).ToString("dd.MM.yyyy")}"); // Ausgabe Zahlungsziel

                        #endregion Rechnung ausstellen

                        break;
                    case "4": //Ausgabe aller gespeicherten Lieferscheine

                        #region Daten ausgeben

                        counterLieferschein = 1;
                        Console.WriteLine("Ausgabe Lieferscheine");

                        foreach (var lieferschein in daten) //Durchlaufen aller Lieferscheine
                        {
                            if (lieferschein != null)//Prüfen ob Lieferschein vorhanden ist
                            {
                                //Ausgabe Kopfzeile Lieferscheine
                                Console.WriteLine("\n--------------------------\n");
                                Console.WriteLine($"Lieferschein {counterLieferschein}");
                                counterLieferschein++;
                                Console.WriteLine($"{"Pos",-3} | {"Bezeichnung",-25} | {"Eur/Stck",-10} | {"Menge",-10}");
                                Console.WriteLine("_______________________________________________________________________");
                                counterLieferscheinPositionen = 1;
                                foreach (var item in lieferschein) //Durchlaufe alle Positionen im Lieferschein
                                {
                                    if (item != null)
                                    {
                                        string[] fragments = item.Split(';');
                                        //                          Pos                             Bezeichnung     Stückpreis      Menge
                                        Console.WriteLine($"{counterLieferscheinPositionen,-3} | {fragments[0],-25} | {fragments[1],-10} | {fragments[2],-10}");
                                        counterLieferscheinPositionen++;
                                    }
                                }
                                Console.WriteLine("_______________________________________________________________________");
                            }
                        }
                        #endregion Daten ausgeben
                        break;
                    case "h": //Hilfe
                        #region Hilfe
                        Console.WriteLine("##################################################");
                        Console.WriteLine($"{"Hilfe",50 / 2 + 5 / 2}");
                        Console.WriteLine("##################################################");
                        Console.WriteLine("");
                        Console.WriteLine("1 - Artikel hinzufügen");
                        Console.WriteLine("Geben Sie eine Lieferscheinnummer ein, der Sie\n  einen Artikel hinzufügen möchten.");
                        Console.WriteLine("Um einen neuen Lieferschein zu erstellen geben\n  Sie ein 0 an, Sie bekommen dann automatisch\n  eine Lieferscheinnummer zugewiesen.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("2 - Lieferschein ausgeben");
                        Console.WriteLine("Geben Sie eine Lieferscheinnummer an um die\n  Positionen des Lieferscheines anzuzeigen.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("3 - Rechnung erstellen");
                        Console.WriteLine("Geben Sie eine Lieferscheinnummer an, zu der\n  Sie eine Rechnung erstellen möchten.");
                        Console.WriteLine("Die Rechnung wird erstellt und Ihnen dann\n  angezeigt.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("4 - Alles Ausgeben");
                        Console.WriteLine("Gibt Ihnen alle aktuellen Lieferscheine mit\n  Lieferscheinnummer un den zugehörigen\n  Positionen aus.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("h - Hilfe");
                        Console.WriteLine("Zeigt Ihnen diese Hileübersicht an.");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("x - Programm beenden");
                        Console.WriteLine("Beendet das Programm, alle nicht\n  gespeicherten Rechnungen gehen verloren");
                        Console.WriteLine("");
                        Console.WriteLine("##################################################");
                        Console.WriteLine("");
                        Console.WriteLine("Haben Sie weiter Fragen?");
                        Console.WriteLine("Kontaktieren Sie uns unter");
                        Console.WriteLine("Tel: 02214 710-700");
                        Console.WriteLine("Mail: support@ck-software.de");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("© CK Software - 2025");
                        #endregion Hilfe
                        break;

                    case "x": //Programm beenden
                        Console.WriteLine("Das Programm wurde beendet.");
                        break;
                    default:
                        break;
                }
                // Auf Nutzereingabe warten
                Console.WriteLine("\n\nDrücken Sie eine belibige Taste zum fortfahren ...");
                Console.ReadKey();
                Console.Clear();
            }
            while (uiMenu != "x"); // Menü wird aufgerufen bis die Eingabe x war.
            Console.ReadKey();

        }
    }
}
