"use client";

import { Rocket } from "lucide-react";
import Link from "next/link";

import { Button } from "@/components/ui/button";
import { useAuth } from "@/features/auth/auth";
import { routes } from "@/lib/routes";

export default function Home() {
  useAuth("guest");

  return (
    <main className="mx-auto max-w-xl">
      <h1 className="text-3xl font-semibold tracking-tight">
        Unlock the Power of Your Telegram Chats
      </h1>

      <p className="text-muted-foreground mt-2 text-lg sm:text-xl">
        Discover insights, trends, and patterns in your group conversations.
        Visualize engagement, top contributors, message frequency, and more —
        all in one sleek dashboard.
      </p>

      <div className="mt-4 flex gap-4">
        <Button size="lg" asChild>
          <Link href={routes.register}>
            <Rocket />
            Analyze My Chat
          </Link>
        </Button>
      </div>
    </main>
  );
}
