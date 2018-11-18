# WhatsBookSharp

## What is WhatsBookSharp?
WhatsBookSharp is a framework written in C# which converts a WhatsApp chat into a tex file.

## Getting started
First, clone the repository using git (recommended):

```bash
git clone https://github.com/Terge3141/WhatsBookSharp.git
``` 

Get the emojis from noto-emoji using git (recommended)

```bash
git clone https://github.com/googlei18n/noto-emoji.git
``` 

Create an directory (from now on referred as $dir)

Export a chat from WhatsApp and save it to the directory ($dir/chat). This directory should contain a .txt file which usually has the name "WhatsApp Chat with The Nickname.txt" and some images.

Open *WhatsBookSharp.sln* and compile it.

Create the tex file

```bash
WhatsBookSharp.exe -i $dir -e /path/to/noto-emoji/png/128
```

Compile the tex file stored in $dir using pdflatex

```bash
pdflatex "WhatsApp Chat with The Nickname.tex"
```

## Image Pools
For some chats it appears not be possible to export them with media. In this case an `<`Media omitted`>` line occurs in the chat file. In this case WhatsBookSharp automatically searches the image pool directory and lists all possible images for the date of interest.

The match file is written to $dir. This can be editted written to the directory $dir/config. When WhatsBooksharp is invoked next time, it read match file and uses it to match $lt`<`Media omitted`>` messages.

The image pool directory should contain *all* images. It is provided as with the command line argument --imagepooldir.
