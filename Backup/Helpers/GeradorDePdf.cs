using System;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace NotaAzul.Helpers
{
    public class GeradorDePdf
    {
        /// <summary>
        /// Cria um pdf a partir de um arquivo .html, utilizando o phantomjs e o arquivo .js rasterize
        /// </summary>
        /// <param name="disco">Disco a ser utilizada C: ou D: por exemplo</param>
        /// <param name="enderecoServer">Local em disco do phantomjs</param>
        /// <param name="url">Endereco da página/arquivo .html</param>
        /// <param name="formato">O formato no qual o pdf será criado.Ex:\"A4\", \"Letter\",\"landscape\"</param>
        /// <param name="nomeArquivo"></param>
        /// <returns></returns>
        public void CriarPdfDeHtml(String disco,String enderecoServer,String url,String nomeArquivo,String formatoPagina,String orientacao)
        {
            DirectoryInfo dir = new DirectoryInfo(@enderecoServer);
            if (dir.GetFiles().Length == 0)
            {
                String sourcePath = @Helpers.Settings.EnderecoPhantomJs();
                String targetPath = @enderecoServer;
                String nomeDoArquivo, arquivoDestino;
                String[] files = Directory.GetFiles(sourcePath);                

                // Copia todas os arquivos do endereço de origem para o destino
                foreach (string file in files)
                {
                    nomeDoArquivo = Path.GetFileName(file);
                    arquivoDestino = Path.Combine(targetPath, nomeDoArquivo);
                    File.Copy(file, arquivoDestino, true);
                }    
            }
            new Thread(new ParameterizedThreadStart(x =>
            {
                ExecuteCommand(disco+" & cd " + enderecoServer + "& phantomjs rasterize.js " + url + " ../relatorios/" + nomeArquivo + ".pdf "+formatoPagina +" "+ orientacao);
            })).Start();

            return;
        }       
         
        /// <summary>
        /// Executa um comando no prompt
        /// </summary>
        /// <param name="command"></param>        
        /// <returns></returns>
        private void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo processInfo;
                Process process;

                processInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;

                process = Process.Start(processInfo);
                process.WaitForExit();
                
            }
            catch { }
        }
       
    }
}