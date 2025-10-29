using DrieLagenMetSQL.Domain;

namespace DrieLagenMetSQL.Presentation
{
    /// <summary>
    /// Applicatie-coördinator voor de console-app.
    /// Stuurt de UI aan en spreekt de DomainController aan.
    /// Bevat geen business- of opslaglogica.
    /// </summary>

    public class DrieLagenMetSQLApplication
    {
        private readonly DomainController _controller;
        private readonly ConsolePresentation _ui;

        public DrieLagenMetSQLApplication(DomainController controller, ConsolePresentation ui)
        {
            ArgumentNullException.ThrowIfNull(controller);
            ArgumentNullException.ThrowIfNull(ui);
            _controller = controller;
            _ui = ui;
        }

        /// <summary>
        /// Start de applicatie-loop.
        /// Toont het hoofdmenu, verwerkt keuzes en handelt fouten af.
        /// </summary>
        public void Run()
        {
            Console.WriteLine("DrieLagenMetSQL demo – welkom\n");

            bool running = true;
            while (running)
            {
                try
                {
                    var keuze = ConsolePresentation.ToonMenuEnLeesKeuze();

                    switch (keuze)
                    {
                        case 'L':
                            _ui.ToonAlleProducten();
                            break;

                        case 'A':
                            _ui.VoegProductToeInteractief();
                            break;

                        case 'U':
                            _ui.UpdateProductInteractief();
                            break;

                        case 'D':
                            _ui.VerwijderProductInteractief();
                            break;

                        case 'Q':
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Onbekende keuze.");
                            break;
                    }
                }
                catch (ApplicationException ex)
                {
                    // Toon de hoofdfout en, indien aanwezig, de inner DB-fout.
                    Console.WriteLine(
                        $"Fout: {ex.Message}" +
                        (ex.InnerException != null ? $" - {ex.InnerException.Message}" : "") +
                        "\n");
                }
                catch (Exception ex)
                {
                    // Vangt onverwachte fouten op (niet-domeinspecifiek).
                    Console.WriteLine($"Onverwachte fout: {ex.Message}\n");
                }
            }

            Console.WriteLine("Tot ziens!\n");
        }
    }
}