using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace proyectoprogra
{
    internal class Program
    {
            public enum Menu
            {
                Consultar = 1, Depositardinero, Retirardinero, Revisarhistorialdedipositivos, Revisarhistorialderetiros,Depositosretiros, Salir
            }
            static double saldo = 0;
            static Dictionary<DateTime, double> historialDepositos = new Dictionary<DateTime, double>();
            static Dictionary<DateTime, double> historialRetiros = new Dictionary<DateTime, double>();


            static void Main(string[] args)
            {
                int Intentos = 3;
                do
                {
                    if (loggin())
                    {
                        Console.WriteLine("Bienvenido a tu banco Santander");

                        while (true)
                        {
                            switch (Mostrarmenu())
                            {
                                case Menu.Consultar:
                                    Consultarsaldo();
                                    break;

                                case Menu.Depositardinero:
                                    Depositardinero();

                                    Console.WriteLine();
                                    break;

                                case Menu.Retirardinero:
                                    retirar();
                                    break;

                                case Menu.Revisarhistorialdedipositivos:
                                    MostrarHistorialDepositos();
                                    break;

                                case Menu.Revisarhistorialderetiros:
                                    MostrarHistorialRetiros();
                                    break;

                                case Menu.Depositosretiros:
                                Depositosoretiros();
                                break;

                                case Menu.Salir:
                                    salir();
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Intentos--;
                        Console.WriteLine($"Fallaste te quedan {Intentos}");
                    }
                    if (Intentos == 0)
                    {
                        Console.WriteLine("Errror");
                        Console.ReadKey();
                        Environment.Exit(1);

                    }

                } while (Intentos >= 0);
            }
            static bool loggin()
            {
                Console.WriteLine("Dame usuario");
                string usuario = Console.ReadLine();
                Console.WriteLine("Dame tu contrasaeña");
                string password = Console.ReadLine();
                Console.WriteLine("Dame tu fecha de cumpleaños");
                DateTime fechaactual = DateTime.Now;
                DateTime fechanaciemienti = Convert.ToDateTime(Console.ReadLine());
                int años = fechaactual.Year - fechanaciemienti.Year;
                if (usuario == "Ronaldo" && password == "123" && años >= 18)
                    return true;

                else
                    return false;




            }
            static Menu Mostrarmenu()
            {
                Console.WriteLine("-------------Menu-------------");
                Console.WriteLine("1) Consultar saldo actual");
                Console.WriteLine("2) Depositar dinero");
                Console.WriteLine("3) Retirar dinero");
                Console.WriteLine("4) Revisar historial de depositos");
                Console.WriteLine("5) Revisar historial de retiros");
                Console.WriteLine("6) Depositos o retiros");
                Console.WriteLine("7) Salir");
                Menu opc = (Menu)Convert.ToInt32(Console.ReadLine());
                return opc;
            }
            static void Consultarsaldo()
            {
                Console.WriteLine($"Su saldo es de {saldo}");
            }
            static void enviarcomprobante(string tipo, double dinero, DateTime fecha)
            {
                Console.Write("¿Quieres que enviemos un comprobante por correo? (s/n): ");
                string respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    correo(tipo, dinero, fecha);
                }
            }
            static void correo(string tipo, double dinero, DateTime fecha)
            {


                string remitente = "113406@alumnouninter.mx";
                string contraseña = "Pochisrena15";
                string destinatario = "nunezron859@gmail.com";

                StringBuilder cuerpo = new StringBuilder();
                cuerpo.AppendLine(" Comprobante de transacción");
                cuerpo.AppendLine($"Tipo de operación: {tipo}");
                cuerpo.AppendLine($"Monto: ${dinero}");
                cuerpo.AppendLine($"Fecha y hora: {fecha}");

                MailMessage mensaje = new MailMessage(remitente, destinatario, $"Comprobante de {tipo}", cuerpo.ToString());

                SmtpClient cliente = new SmtpClient("smtp.office365.com", 587)
                {
                    Credentials = new NetworkCredential(remitente, contraseña),
                    EnableSsl = true
                };

                try
                {
                    cliente.Send(mensaje);
                    Console.WriteLine("Comprobante enviado exitosamente al correo.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Error al enviar el comprobante: " + ex.Message);
                }
            }

            static void Depositardinero()
            {
                Console.WriteLine("Ingresar dinero a depositar");
                double dinero = Convert.ToDouble(Console.ReadLine());
                saldo += dinero;
                DateTime fecha = DateTime.Now;
                historialDepositos.Add(fecha, dinero);
                Console.WriteLine("Depósito correcto");
                enviarcomprobante("Depósito", dinero, fecha);
            }

            static void retirar()
            {
                Console.WriteLine("Ingresa dinero a retirar");
                double dinero = Convert.ToDouble(Console.ReadLine());
                if (saldo >= dinero)
                {
                    saldo -= dinero;
                    DateTime fecha = DateTime.Now;
                    historialRetiros.Add(fecha, dinero);
                    Console.WriteLine("Retiro correcto");
                    enviarcomprobante("Retiro", dinero, fecha);
                }
                else
                {
                    Console.WriteLine("Error: saldo insuficiente para hacer el retiro");
                }
            }

            static void MostrarHistorialDepositos()
            {
                Console.WriteLine("Historial de Depósitos");

                if (historialDepositos.Count == 0)
                {
                    Console.WriteLine("No se han realizado depósitos aún");
                }
                else
                {
                    foreach (var d in historialDepositos)
                    {
                        Console.WriteLine($"Fecha: {d.Key} | Dinero: ${d.Value}");
                    }
                }
            }
            static void MostrarHistorialRetiros()
            {
                Console.WriteLine("--- Historial de Retiros ---");

                if (historialRetiros.Count == 0)
                {
                    Console.WriteLine("No se han realizado retiros aún.");
                }
                else
                {
                    foreach (var r in historialRetiros)
                    {
                        Console.WriteLine($"Fecha: {r.Key} | Dinero: ${r.Value}");
                    }
                }
            }
            static void Depositosoretiros()
            {
            Console.WriteLine("¿Que hay más?"); 
          
            if (historialDepositos.Count > historialRetiros.Count)
            {
                Console.WriteLine($"Hay más depositos");
                Console.WriteLine($"D : {historialDepositos.Count}");
                Console.WriteLine($"R : {historialRetiros.Count}");
            }

            else 
            {
                Console.WriteLine($"Hay mas retiros");
                Console.WriteLine($"D : {historialDepositos.Count}");
                Console.WriteLine($"R : {historialRetiros.Count}");
            }
            }
        
            static void salir()
            {
                Console.WriteLine("Hasta pronto");
                Environment.Exit(0);
            }



        }
    }




       
 

