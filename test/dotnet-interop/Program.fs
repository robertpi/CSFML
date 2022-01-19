open System.Net.Http
open System.Threading
open SFML.Audio

let private waitFinish (m: Music)  =
    let rec loop() =
        if m.Status <> SoundStatus.Stopped then
            Thread.Sleep(System.TimeSpan.FromMilliseconds 100)
            loop()
    loop()

let main() =
    let flow =
        async {
            let client = new HttpClient()
            let! response = client.GetAsync("http://horton.com/consulting/portfolio/dwbt/bouncer/media/sample.wav")  |> Async.AwaitTask
            let _ = response.EnsureSuccessStatusCode()
            let! responseBody = response.Content.ReadAsByteArrayAsync() |> Async.AwaitTask
            return responseBody
        }

    let wav = flow |> Async.RunSynchronously
    use m = new Music(wav)
    m.Play()
    waitFinish m

main()